using System;
using System.Collections.Generic;

#nullable disable

namespace VerificationsService.Models.DBModel
{
    public partial class SmsChallenge
    {
        public int SmsChallengeId { get; set; }
        public int? ChallengeId { get; set; }
        public string ReceiverNumber { get; set; }

        public virtual Challenge Challenge { get; set; }
    }
}
