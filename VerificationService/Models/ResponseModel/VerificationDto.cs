using System.ComponentModel.DataAnnotations;

namespace VerificationService.Models.ResponseModel
{
    public class VerificationDto
    {
        [Required(ErrorMessage = "Please provide your OTP!!")]
        public string otp { get; set; }
        public string challengeId { get; set; }
    }
}
