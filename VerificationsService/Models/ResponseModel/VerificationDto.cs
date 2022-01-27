using System;
using System.ComponentModel.DataAnnotations;

namespace VerificationsService.Models.ResponseModel
{
    public class VerificationsDto
    {
        [Required(ErrorMessage = "Please provide your OTP!!")]
        public string otp { get; set; }
        public string challengeId { get; set; }
    }

    [Serializable]
    class LimitExceedException : Exception
    {
        public LimitExceedException()
        {

        }
    }

    [Serializable]
    class CodeAlreadyUsedException : Exception
    {
        public CodeAlreadyUsedException()
        {

        }
    }

    [Serializable]
    class CodeInvalidException : Exception
    {
        public CodeInvalidException()
        {

        }
        public CodeInvalidException(string ErrorMessage) : base(ErrorMessage)
        {

        }
    }


}