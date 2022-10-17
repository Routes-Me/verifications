using System;
using System.Linq;
using System.Threading.Tasks;
using VerificationsService.Abstraction;
using VerificationsService.Models.DBModel;

namespace VerificationsService.Repository
{
    public class VerificationRepository : GenericRepository<Verification>, IVerificationRepository
    {
        private readonly IGenericRepository<Verification> _verificationRepository;
        private readonly IGenericRepository<Challenge> _challengeRepository;
        private readonly IGenericRepository<SmsChallenge> _smsChallengeRepository;
        public VerificationRepository(verificationsdatabaseContext context) : base(context)
        {
            _verificationRepository = new GenericRepository<Verification>(context);
            _challengeRepository = new GenericRepository<Challenge>(context);
            _smsChallengeRepository = new GenericRepository<SmsChallenge>(context);
        }

        public int CheckAttemptCount(int challenge_id)
        {
            return _verificationRepository.Find(x => x.ChallengeId == challenge_id).Result.Count();
        }
        public async Task RemoveALLFailedVerifications()
        {
            DateTime currentTime = DateTime.Now.AddMinutes(-10);
            var expiredChallenges = await _challengeRepository.Find(c => c.ExpiresAt < currentTime);

            foreach (Challenge challenge in expiredChallenges)
            {
                var expiredVerifications = await _verificationRepository.Find(v => v.ChallengeId == challenge.ChallengeId);
                foreach (Verification item in expiredVerifications)
                {
                    await _verificationRepository.Delete(item.VerificationId);
                }
                var expiredSMSChallenges = await _smsChallengeRepository.Find(s => s.ChallengeId == challenge.ChallengeId);
                foreach (SmsChallenge item in expiredSMSChallenges)
                {
                    await _challengeRepository.Delete(item.SmsChallengeId);
                }
                await _challengeRepository.Delete(challenge.ChallengeId);

            }
        }
    }
}
