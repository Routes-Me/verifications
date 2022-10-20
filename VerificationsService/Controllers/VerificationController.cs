using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RoutesSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerificationsService.Abstraction;
using VerificationsService.Models.Common;
using VerificationsService.Models.DBModel;
using VerificationsService.Models.ResponseModel;
using static VerificationsService.Models.ResponseModel.Response;

namespace VerificationsService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationRepository _verificationRepository;
        private readonly IChallengeRepository _challengeRepository;
        private readonly ITokenService _tokenService;

        public VerificationController(IVerificationRepository verificationRepository, IChallengeRepository challengeRepository, ITokenService tokenService)
        {
            _verificationRepository = verificationRepository;
            _challengeRepository = challengeRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("verification-challenges/{challenge_id}/verifications/")]
        public async Task<IActionResult> VerifyOTP([FromRoute] string challenge_id, [FromBody] VerificationDto verificationDto)
        {
            try
            {
                if (string.IsNullOrEmpty(challenge_id))
                    throw new ArgumentNullException("Invalid Details!!");

                if (string.IsNullOrEmpty(verificationDto.otp))
                    throw new ArgumentNullException("Provide your OTP to proceed!!");

                verificationDto.challengeId = challenge_id;
                var challenge = await _challengeRepository.GetChallengeForId(Obfuscation.Decode(verificationDto.challengeId));

                if (challenge.Status == ChallengeStatus.Pending)
                {
                    challenge.Status = ChallengeStatus.Approved;
                }

                if (challenge == null)
                    throw new KeyNotFoundException($"The given challenge id '{challenge_id}' doesnot exist");
                else
                {
                    if (challenge.ExpiresAt < DateTime.Now)
                        return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.TokenExpired, 400));

                    else
                    {
                        var count = _verificationRepository.CheckAttemptCount(Obfuscation.Decode(challenge_id));
                        if (count >= VerificationConfig.MaxVerificationAttempts)
                        {
                            if (challenge.Status == ChallengeStatus.Approved)
                            {
                                challenge.Status = ChallengeStatus.Denied;
                                challenge = _challengeRepository.Where(x => x.ChallengeId == Obfuscation.Decode(challenge_id));
                                await _challengeRepository.Put(challenge);
                            }
                            return StatusCode(StatusCodes.Status429TooManyRequests, ReturnResponse.ErrorResponse(CommonMessage.LimitExceedException, 429));
                        }
                        Verification verification = new Verification
                        {
                            ChallengeId = challenge.ChallengeId,
                            CreatedAt = DateTime.Now,
                            Attempt = challenge.Code == verificationDto.otp ? VerificationStatus.successful : VerificationStatus.failure

                        };
                        await _verificationRepository.Add(verification);
                        var verificationResponse = new VerificationResponse();
                        if (verification.Attempt != VerificationStatus.failure)
                        {
                            var sms = _challengeRepository.GetSmsChallengeForChallengeId(verification.ChallengeId);
                            verificationResponse.Token = _tokenService.GenerateVerificationTokenForNumber(sms.ReceiverNumber);
                            verificationResponse.Status = true;
                            verificationResponse.Code = 200;
                            verificationResponse.Message = CommonMessage.TokenCreated;
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.IncorrectOTP, 400));
                        }
                        return StatusCode(StatusCodes.Status200OK, verificationResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ReturnResponse.ExceptionResponse(ex));
            }
        }
    }
}