namespace VerificationService.Abstraction
{
    public interface ITokenService
    {
        public string GenerateVerificationTokenForNumber(string phoneNumber);
    }
}
