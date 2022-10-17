using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static VerificationsService.Models.ResponseModel.Response;
using System.Threading.Tasks;
using System;
using VerificationsService.Models.ResponseModel;
using VerificationsService.Abstraction;
using VerificationsService.Models.DBModel;

namespace VerificationsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class ChallengeController : ControllerBase
    {
        private readonly IChallengeRepository _challengeRepository;

        public ChallengeController(IChallengeRepository challengeRepository)
        {
            _challengeRepository = challengeRepository;
        }

        [HttpPost]
        [Route("verification-challenges")]
        public async Task<IActionResult> Post([FromBody] ChallengeDto challenge)
        {
            try
            {
                var newChallenge = await _challengeRepository.CreateChallenge(challenge);
                return StatusCode(StatusCodes.Status200OK, newChallenge);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ReturnResponse.ExceptionResponse(ex));
            }
        }
    }
}
