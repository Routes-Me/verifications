using System;
using System.Threading.Tasks;
using VerificationsService.Models.Common;
using VerificationsService.Models.ResponseModel;
using static MessagingLibraries.MessagingLibrary;
namespace VerificationsService.Helpers
{
    public class ServiceHelper
    {
        public async Task<string> SendOTPToUser(ChallengeDto challenge)
        {
            var otp = CreateOTPForChannel(challenge.Channel);
            try
            {
                if (challenge.PhoneNumber.Substring(0, 3) == "965")
                {
                    var result = await SendSMS(challenge.PhoneNumber, otp, challenge.LangId);
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

        public string CreateOTPForChannel(string channel)
        {
            string OTP = "";

            if (channel == VerificationConfig.SMSChannel)
                OTP = GenerateRandomOTP(VerificationConfig.OTPLengthForSMS, VerificationConfig.OTPInputArray);

            if (channel == VerificationConfig.EmailChannel)
                OTP = GenerateRandomOTP(VerificationConfig.OTPLengthForMail, VerificationConfig.OTPInputArray);

            return OTP;
        }

        public string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
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
