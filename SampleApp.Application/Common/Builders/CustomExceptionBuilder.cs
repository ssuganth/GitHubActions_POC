using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Data.SqlClient;
using SampleApp.Application.Common.Enums;
using SampleApp.Application.Internal;
using SampleApp.Data;

namespace SampleApp.Application.Common.Builders
{
    public class CustomExceptionBuilder : ICustomExceptionBuilder
    {
        public ResponseModel<T> BuildIdenticalPrimaryKeyException<T>(Exception e, ResponseModel<T> response, string id)
        {
            if (e.InnerException is SqlException sqlException &&
                (sqlException.Number == 2601 || sqlException.Number == 2627))
            {
                response.Status = HttpStatusCode.Conflict;
                response.Errors = new List<ErrorResponse>()
                {
                    new ErrorResponse()
                    {
                        Reason = (int)ErrorTypes.AlreadyExists,
                        Message = $"{id} is already created"
                    }
                };
                return response;
            }

            return default;
        }

        public ResponseModel<T> BuildEntityNotFoundException<T>(ResponseModel<T> response, string id, ErrorTypes errorType)
        {
            response.Status = HttpStatusCode.NotFound;
            response.Errors = new List<ErrorResponse>()
            {
                new ErrorResponse()
                {
                    Reason = errorType.EnumToInt(),
                    Message = $"{id} is not found"
                }
            };

            return response;
        }

        public ResponseModel<T> BuildMaliciousInputFoundException<T>(ResponseModel<T> response, string input, ErrorTypes errorType)
        {
            response.Status = HttpStatusCode.BadRequest;
            response.Errors = new List<ErrorResponse>()
            {
                new ErrorResponse()
                {
                    Reason = errorType.EnumToInt(),
                    Message = $"{input} is malicious"
                }
            };

            return response;
        }
    }
}