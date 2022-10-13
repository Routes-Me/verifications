using System.Threading.Tasks;
using VerificationService.Models.DBModel;

namespace VerificationService.Abstraction
{
    public interface IVerificationRepository : IGenericRepository<Verification>
    {
        int CheckAttemptCount(int challenge_id);
        Task RemoveALLFailedVerifications();
    }
}
