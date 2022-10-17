namespace VerificationsService.Abstraction
{
    public interface ITokenService
    {
        public string GenerateVerificationTokenForNumber(string phoneNumber);
    }
}
