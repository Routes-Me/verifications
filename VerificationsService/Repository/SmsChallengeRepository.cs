using VerificationsService.Abstraction;
using VerificationsService.Models.DBModel;

namespace VerificationsService.Repository
{
    public class SmsChallengeRepository : GenericRepository<SmsChallenge>, ISmsChallengeRepository
    {
        private readonly IGenericRepository<SmsChallenge> _smsChallenge;

        public SmsChallengeRepository(verificationsdatabaseContext context) : base(context)
        {
            _smsChallenge = new GenericRepository<SmsChallenge>(context);
        }
    }
}
