using SampleApp.Data;
using System;
using SampleApp.Application.Common.Enums;

namespace SampleApp.Application.Common.Builders
{
    public interface ICustomExceptionBuilder
    {
        ResponseModel<T> BuildIdenticalPrimaryKeyException<T>(Exception e, ResponseModel<T> response, string id);
        ResponseModel<T> BuildEntityNotFoundException<T>(ResponseModel<T> response, string id, ErrorTypes errorType);
        ResponseModel<T> BuildMaliciousInputFoundException<T>(ResponseModel<T> response, string input, ErrorTypes errorType);
    }
}