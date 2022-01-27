using System;
using System.Collections.Generic;

#nullable disable

namespace VerificationsService.Models.DBModel
{
    public partial class Challenge
    {
        public Challenge()
        {
            SmsChallenges = new HashSet<SmsChallenge>();
            Verifications = new HashSet<Verification>();
        }

        public int ChallengeId { get; set; }
        public string Channel { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public virtual ICollection<SmsChallenge> SmsChallenges { get; set; }
        public virtual ICollection<Verification> Verifications { get; set; }
    }
}
