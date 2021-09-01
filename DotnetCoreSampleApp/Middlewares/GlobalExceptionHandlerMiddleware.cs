using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SampleApp.Data;

namespace DotnetCoreSampleApp.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case AggregateException aggregateException:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonConvert.SerializeObject(aggregateException.StackTrace);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new ResponseModel<bool>()
                {
                    IsSuccessful = false,
                    Status = code,
                    Result = false,
                    Errors = new List<ErrorResponse>()
                    {
                        new ErrorResponse()
                        {
                            Reason = (int)code,
                            Message = exception.Message
                        }
                    }
                });
            }

            return context.Response.WriteAsync(result);
        }
    }
}