using System;
using System.Threading.Tasks;
using RoutesSecurity;
using VerificationsService.Abstraction;
using VerificationsService.Models.Common;
using VerificationsService.Models.DBModel;
using VerificationsService.Models.ResponseModel;
using VerificationsService.Service;
using static MessagingLibraries.MessagingLibrary;

namespace MessagingService.Service
{
    public interface IVerificationService
    {
        Task<ChallengeResponse> CreateChallenge(ChallengeDto challenge);
        Task<VerificationResponse> VerifyChallenge(VerificationsDto verify);
    }

    public class VerificationService : IVerificationService
    {
        private readonly ITokenService _tokenService;
    
        private readonly IVerificationsRepository _verificationRepository;
        public VerificationService(ITokenService tokenService, 
                                   IVerificationsRepository verificationRepository)
        {
            _tokenService = tokenService;
            _verificationRepository = verificationRepository;
        }
        public async Task<ChallengeResponse> CreateChallenge(ChallengeDto challenge)
        {
            ValidateChallenge(challenge);

            Challenge newChallenge = await HandleChallengeForChannel(challenge);

            ChallengeResponse response = new ChallengeResponse();
            response.ChallengeId = Obfuscation.Encode(newChallenge.ChallengeId);

            return response;
        }

        public async Task<VerificationResponse> VerifyChallenge(VerificationsDto verify)
        {
            ValidateVerification(verify);

            Challenge challengeToVerify = await _verificationRepository.getChallengeForId(Obfuscation.Decode(verify.challengeId));

            ValidateVerificationForChallenge(challengeToVerify);

            Verification verification = verifyChallengeForOTP(challengeToVerify, verify.otp);

            Verification newVerification = await _verificationRepository.insertVerification(verification);

            if (newVerification.Attempt == VerificationStatus.failure)
                throw new AccessViolationException();

            VerificationResponse response = new VerificationResponse();
            SmsChallenge smsChallenge = await _verificationRepository.getSmsChallengeForChallengeId(verification.ChallengeId);
            response.Token = _tokenService.GenerateVerificationTokenForNumber(smsChallenge.ReceiverNumber);

            return response;
        }

        
        private async void ValidateVerificationForChallenge(Challenge challengeToVerify)
        {
            if (challengeToVerify == null)
                throw new AccessViolationException("Invalid challenge!!");

            DateTime currentTime = DateTime.Now;
            if (challengeToVerify.ExpiresAt < currentTime)
                throw new CodeInvalidException();

            await _verificationRepository.checkAttemptsForChallengeId(challengeToVerify.ChallengeId);

        }

        private void ValidateChallenge(ChallengeDto challenge)
        {
            if (string.IsNullOrEmpty(challenge.PhoneNumber))
                throw new ArgumentNullException("Phone number missing!!");

            if (string.IsNullOrEmpty(challenge.Channel))
                throw new ArgumentNullException("Provide a channel for your request");
        }

         private void ValidateVerification(VerificationsDto verify)
        {
            if (string.IsNullOrEmpty(verify.challengeId))
                throw new ArgumentNullException("Invalid Details!!");

            if (string.IsNullOrEmpty(verify.otp))
                throw new ArgumentNullException("Provide your OTP to proceed!!");
        }

        private Verification verifyChallengeForOTP(Challenge challenge, string OTP)
        {
            DateTime currentTime = DateTime.Now;
            Verification verify = new Verification();
            verify.ChallengeId = challenge.ChallengeId;
            verify.CreatedAt = currentTime;
            verify.Attempt = VerificationStatus.failure;

            if (challenge.Code == OTP)
                verify.Attempt = VerificationStatus.successful;

            return verify;
        }

        // private async void ValidateVerificationForChallenge(Challenge challengeToVerify)
        // {
        //     if (challengeToVerify == null)
        //         throw new AccessViolationException("Invalid challenge!!");

        //     DateTime currentTime = DateTime.Now;
        //     if (challengeToVerify.ExpiresAt < currentTime)
        //         throw new CodeInvalidException();

        //     await checkAttemptsForChallengeId(challengeToVerify.ChallengeId);

        // }

        // private async Task checkAttemptsForChallengeId(int? challenge_id)
        // {
        //     var Attempts = await _context.Verifications.Where(v => v.ChallengeId == challenge_id).ToListAsync();

        //     if (Attempts.Where(c => c.Attempt == VerificationStatus.successful).FirstOrDefault() != null)
        //         throw new CodeAlreadyUsedException();

        //     if (Attempts.Count() >= VerificationConfig.MaxVerificationAttempts)
        //         throw new LimitExceedException();
        // }

        private async Task<Challenge> HandleChallengeForChannel(ChallengeDto challenge)
        {
            Challenge newChallenge = new Challenge();
            if (challenge.Channel == VerificationConfig.SMSChannel)
            {
                string sentOTP = await sendOTPToUser(challenge);
                newChallenge = await _verificationRepository.insertChallengeForOTP(sentOTP);
                await _verificationRepository.insertSmsChallenge(challenge.PhoneNumber, newChallenge.ChallengeId);
            }
            return newChallenge;
        }

        private async Task<string> sendOTPToUser(ChallengeDto verify)
        {
            var otp = createOTPForChannel(verify.Channel);
            try
            {
                if (verify.PhoneNumber.Substring(0, 3) == "965")
                {
                    var result = await SendSMS(verify.PhoneNumber, otp, verify.LangId);
                    if (result.Substring(0, 3) == "ERR")
                        throw new Exception(" Messaging Error : " + result.Split(":")[1]);
                    return otp;
                }
                else
                {
                    throw new Exception("ErrorMessage: Please provide a valid Kuwait number!!.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ErrorMessage: " + ex.Message);
            }
        }

        private string createOTPForChannel(string channel)
        {
            string OTP = "";

            if (channel == VerificationConfig.SMSChannel)
                OTP = GenerateRandomOTP(VerificationConfig.OTPLengthForSMS, VerificationConfig.OTPInputArray);

            if (channel == VerificationConfig.EmailChannel)
                OTP = GenerateRandomOTP(VerificationConfig.OTPLengthForMail, VerificationConfig.OTPInputArray);

            return OTP;
        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = "";
            string sTempChars = "";
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        
    }
}