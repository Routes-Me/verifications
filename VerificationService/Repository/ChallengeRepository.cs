using RoutesSecurity;
using System;
using System.Threading.Tasks;
using VerificationService.Abstraction;
using VerificationService.Helpers;
using VerificationService.Models.Common;
using VerificationService.Models.DBModel;
using VerificationService.Models.ResponseModel;
using static VerificationService.Models.ResponseModel.Response;

namespace VerificationService.Repository
{
    public class ChallengeRepository : GenericRepository<Challenge>, IChallengeRepository
    {
        private readonly IGenericRepository<Challenge> _challengeRepository;
        private readonly IGenericRepository<SmsChallenge> _smsChallengeRepository;

        public ChallengeRepository(verificationsdatabaseContext context)
           : base(context)
        {
            _challengeRepository = new GenericRepository<Challenge>(context);
            _smsChallengeRepository = new GenericRepository<SmsChallenge>(context);
        }

        public async Task<Response> CreateChallenge(ChallengeDto challenge)
        {
            if (string.IsNullOrEmpty(challenge.PhoneNumber))
                throw new ArgumentNullException("Phone number missing!!");

            if (string.IsNullOrEmpty(challenge.Channel))
                throw new ArgumentNullException("Provide a channel for your request");

            var newChallenge = await HandleChallengeForChannel(challenge);

            var response = new ChallengeResponse();
            response.Message = CommonMessage.ChallengedIdCreated;
            response.Status = true;
            response.Code = 200;
            response.ChallengeId = Obfuscation.Encode(newChallenge.ChallengeId);
            return response;
        }
        public async Task<Challenge> HandleChallengeForChannel(ChallengeDto challenge)
        {
            Challenge newChallenge = new Challenge();
            if (challenge.Channel == VerificationConfig.SMSChannel)
            {
                string sentOTP = await new ServiceHelper().SendOTPToUser(challenge);
                newChallenge = await InsertChallengeForOTP(sentOTP);
                await InsertSmsChallenge(challenge.PhoneNumber, newChallenge.ChallengeId);
            }
            return newChallenge;
        }
        public async Task<Challenge> InsertChallengeForOTP(string sentOTP)
        {
            Challenge newChallenge = new Challenge();
            newChallenge.Channel = VerificationConfig.SMSChannel;
            newChallenge.Type = VerificationConfig.OTPChallengeType;
            newChallenge.Status = ChallengeStatus.Pending;
            newChallenge.Code = sentOTP;
            newChallenge.UpdatedAt = DateTime.Now;
            newChallenge.ExpiresAt = DateTime.Now.AddMinutes(15);
            newChallenge.CreatedAt = DateTime.Now;
            await _challengeRepository.Add(newChallenge);
            return newChallenge;
        }
        public async Task<SmsChallenge> InsertSmsChallenge(string number, int challenge_id)
        {
            SmsChallenge newSmsChallenge = new SmsChallenge();
            newSmsChallenge.ChallengeId = challenge_id;
            newSmsChallenge.ReceiverNumber = number;
            await _smsChallengeRepository.Add(newSmsChallenge);
            return newSmsChallenge;
        }
        public async Task<Challenge> GetChallengeForId(int challenge_id)
        {
            return await _challengeRepository.GetById(challenge_id);
        }

        public SmsChallenge GetSmsChallengeForChallengeId(int? challenge_id)
        {
            return _smsChallengeRepository.Where(c => c.ChallengeId == challenge_id);

        }

    }
}
