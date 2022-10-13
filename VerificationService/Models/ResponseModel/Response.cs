using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoutesSecurity;
using System;
using System.Collections.Generic;

namespace VerificationService.Models.ResponseModel
{
    public class Response
    {

        public int Code { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public static class ReturnResponse
        {
            public static dynamic ErrorResponse(string message, int statusCode)
            {
                var response = new Response
                {
                    Status = false,
                    Message = message,
                    Code = statusCode
                };
                return response;
            }
            public static dynamic ExceptionResponse(Exception ex)
            {
                var response = new Response
                {
                    Status = false,
                    Message = CommonMessage.ExceptionMessage + ex.Message,
                    Code = StatusCodes.Status500InternalServerError
                };
                return response;
            }
            public static dynamic SuccessResponse(string message, bool isCreated)
            {
                var response = new Response
                {
                    Status = true,
                    Message = message
                };
                if (isCreated)
                {
                    response.Code = StatusCodes.Status201Created;
                }
                else
                    response.Code = StatusCodes.Status200OK;
                return response;
            }
        }
        public class ChallengeResponse : Response
        {
            public string ChallengeId { get; set; }
        }

        public class VerificationResponse : Response
        {
            public string Token { get; set; }
        }
    }
}
