using System.Threading.Tasks;
using VerificationsService.Models.DBModel;

namespace VerificationsService.Abstraction
{
    public interface IVerificationsRepository
    {
        Task<Challenge> insertChallengeForOTP(string code);
        Task<SmsChallenge> insertSmsChallenge(string number, int challenge_id);
        Task<Challenge> getChallengeForId(int challenge_id);
        Task checkAttemptsForChallengeId(int? challenge_id);
        Task<Verification> insertVerification(Verification verify);
        Task<SmsChallenge> getSmsChallengeForChallengeId(int? challenge_id);
        Task RemoveALLFailedVerifications();
    }
}