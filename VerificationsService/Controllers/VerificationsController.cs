using System;
using System.Threading.Tasks;
using MessagingService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VerificationsService.Abstraction;
using VerificationsService.Models.ResponseModel;

namespace VerificationsService.Controllers
{
    public class VerificationsController : ControllerBase
    {
        private readonly ILogger<VerificationsController> _logger;
        private readonly IVerificationService _verificationsService;

        public VerificationsController(IVerificationService verificationsService,
                                       ILogger<VerificationsController> logger)
        {
            _logger = logger;
            _verificationsService = verificationsService;
        }


        [HttpPost]
        [Route("verification-challenges/{challenge_id}/verifications/")]
        public async Task<IActionResult> verifyOTP([FromRoute] string challenge_id, [FromBody] VerificationsDto verify)
        {
            verify.challengeId = challenge_id;

            VerificationResponse newVerification = new VerificationResponse();
            try
            {
                newVerification = await _verificationsService.VerifyChallenge(verify);
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrorResponse { Message = CommonMessage.InvalidData });
            }
            catch (CodeAlreadyUsedException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponse { Message = CommonMessage.CodeAlreadyUsed });
            }
            catch (CodeInvalidException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponse { Message = CommonMessage.CodeInvalid });
            }
            catch (LimitExceedException)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, new ErrorResponse { Message = CommonMessage.LimitExceedException });
            }
            catch (AccessViolationException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new ErrorResponse { Message = CommonMessage.Forbidden });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = CommonMessage.ExceptionMessage + ex.Message });
            }
            return StatusCode(StatusCodes.Status200OK, newVerification);
        }

        [HttpPost]
        [Route("verification-challenges")]
        public async Task<IActionResult> createChallenge([FromBody]ChallengeDto challenge)
        {
            ChallengeResponse newChallenge = new ChallengeResponse();
            try
            {
                newChallenge = await _verificationsService.CreateChallenge(challenge);
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity);
            }
            catch (AccessViolationException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonMessage.ExceptionMessage + ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK, newChallenge);

        }
    }
}