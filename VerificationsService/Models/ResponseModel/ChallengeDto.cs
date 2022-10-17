using System.ComponentModel.DataAnnotations;

namespace VerificationsService.Models.ResponseModel
{
    public class ChallengeDto
    {

        [Required(ErrorMessage = "Please provide a phone number!!")]
        public string PhoneNumber { get; set; }
        public int LangId { get; set; } = 1;
        [Required(ErrorMessage = "Please provide a channel for verification!!")]
        public string Channel { get; set; }

    }
}