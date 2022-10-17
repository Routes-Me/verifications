using System;
using System.Collections.Generic;

#nullable disable

namespace VerificationsService.Models.DBModel
{
    public partial class Verification
    {
        public int VerificationId { get; set; }
        public int? ChallengeId { get; set; }
        public string Attempt { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Challenge Challenge { get; set; }
    }
}
