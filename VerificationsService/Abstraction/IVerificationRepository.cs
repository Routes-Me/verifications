using System.Threading.Tasks;
using VerificationsService.Models.DBModel;

namespace VerificationsService.Abstraction
{
    public interface IVerificationRepository : IGenericRepository<Verification>
    {
        int CheckAttemptCount(int challenge_id);
        Task RemoveALLFailedVerifications();
    }
}
