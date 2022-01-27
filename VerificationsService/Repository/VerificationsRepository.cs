using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VerificationsService.Abstraction;
using VerificationsService.Models.Common;
using VerificationsService.Models.DBModel;
using VerificationsService.Models.ResponseModel;
using VerificationsService.Service;

namespace VerificationsService.Repository
{
    public class VerificationsRepository : IVerificationsRepository
    {
        private readonly AppSettings _appsettings;
        private readonly Dependencies _dependencies;
        private readonly IGenericRepository<Verification> _verificationRepo;
        private readonly IGenericRepository<SmsChallenge> _smsChallengeRepo;
        private readonly IGenericRepository<Challenge> _challengeRepo;
        private readonly ITokenService _tokenService;
        private readonly verificationsdatabaseContext _context;

        public VerificationsRepository(IOptions<AppSettings> appSettings,
                                      IOptions<Dependencies> dependencies,
                                      verificationsdatabaseContext context,
                                      IGenericRepository<Verification> verificationRepo,
                                      IGenericRepository<Challenge> challengeRepo,
                                      IGenericRepository<SmsChallenge> smsChallengeRepo,
                                      ITokenService tokenService)
        {
            _appsettings = appSettings.Value;
            _dependencies = dependencies.Value;
            _verificationRepo = verificationRepo;
            _smsChallengeRepo = smsChallengeRepo;
            _challengeRepo = challengeRepo;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task checkAttemptsForChallengeId(int? challenge_id)
        {
            var Attempts = await _context.Verifications.Where(v => v.ChallengeId == challenge_id).ToListAsync();

            if (Attempts.Where(c => c.Attempt == VerificationStatus.successful).FirstOrDefault() != null)
                throw new CodeAlreadyUsedException();

            if (Attempts.Count() >= VerificationConfig.MaxVerificationAttempts)
                throw new LimitExceedException();
        }

        public async Task<Challenge> getChallengeForId(int challenge_id)
        {
            return await _challengeRepo.GetById(challenge_id);
        }

        public async Task<SmsChallenge> getSmsChallengeForChallengeId(int? challenge_id)
        {
            return await _context.SmsChallenges.FirstOrDefaultAsync(c => c.ChallengeId == challenge_id);

        }

        public async Task<Challenge> insertChallengeForOTP(string code)
        {
            DateTime currentTime = DateTime.Now;
            Challenge newChallenge = new Challenge();
            newChallenge.Channel = VerificationConfig.SMSChannel;
            newChallenge.Type = VerificationConfig.OTPChallengeType;
            newChallenge.Status = ChallengeStatus.Pending;
            newChallenge.Code = code;
            newChallenge.UpdatedAt = currentTime;
            newChallenge.ExpiresAt = currentTime.AddMinutes(15);
            newChallenge.CreatedAt = currentTime;
            await _challengeRepo.Add(newChallenge);
            return newChallenge;
        }

        public async Task<SmsChallenge> insertSmsChallenge(string number, int challenge_id)
        {
            SmsChallenge newSmsChallenge = new SmsChallenge();
            newSmsChallenge.ChallengeId = challenge_id;
            newSmsChallenge.ReceiverNumber = number;
            var dbResponse = await _smsChallengeRepo.Add(newSmsChallenge);
            return newSmsChallenge;
        }

        public async Task<Verification> insertVerification(Verification verify)
        {
            await _verificationRepo.Add(verify);
            return verify;
        }

        public async Task RemoveALLFailedVerifications()
        {
            DateTime currentTime = DateTime.Now;
            List<Challenge> expiredChallenges = await _context.Challenges.Where(c => c.ExpiresAt < currentTime).ToListAsync();

            foreach (Challenge challenge in expiredChallenges)
            {
                List<Verification> expiredVerifications = await _context.Verifications.Where(v => v.ChallengeId == challenge.ChallengeId).ToListAsync();
                foreach (Verification item in expiredVerifications)
                {
                    _context.Verifications.Remove(item);
                }
                List<SmsChallenge> expiredSMSChallenges = await _context.SmsChallenges.Where(s => s.ChallengeId == challenge.ChallengeId).ToListAsync();
                foreach (SmsChallenge item in expiredSMSChallenges)
                {
                    _context.SmsChallenges.Remove(item);
                }
                _context.Challenges.Remove(challenge);
                await _context.SaveChangesAsync();
            }
        }
    }
}