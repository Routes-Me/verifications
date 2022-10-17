using System.Threading.Tasks;
using VerificationsService.Models.DBModel;
using VerificationsService.Models.ResponseModel;

namespace VerificationsService.Abstraction
{
    public interface IChallengeRepository : IGenericRepository<Challenge>
    {
        Task<Response> CreateChallenge(ChallengeDto challenge);
        Task<Challenge> InsertChallengeForOTP(string sentOTP);
        Task<SmsChallenge> InsertSmsChallenge(string phoneNumber, int challengeId);
        Task<Challenge> GetChallengeForId(int challenge_id);
        Task<Challenge> HandleChallengeForChannel(ChallengeDto challenge);
        SmsChallenge GetSmsChallengeForChallengeId(int? challenge_id);
    }
}
