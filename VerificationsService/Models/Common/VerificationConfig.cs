namespace VerificationsService.Models.Common
{
    public class VerificationConfig
    {
        public static string[] OTPInputArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        public static string EmailChannel = "mail";
        public static string SMSChannel = "sms";
        public static int OTPLengthForSMS = 4;
        public static int OTPLengthForMail = 6;
        public static string OTPChallengeType = "otp";
        public static int MaxVerificationAttempts = 3;

    }

    public class ChallengeStatus
    {
        public static string Pending = "pending";
        public static string Approved = "approved";
        public static string Denied = "denied";
    }

    public class VerificationStatus
    {
        public static string successful = "successful";
        public static string failure = "failure";

    }
}