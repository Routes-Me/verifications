namespace VerificationsService.Models.ResponseModel
{
    public class ErrorResponse
    {
        public string Message { get; set; }
    }

    public class ChallengeResponse
    {
        public string ChallengeId { get; set; }
    }

    public class VerificationResponse
    {
        public string Token { get; set; }
    }
}