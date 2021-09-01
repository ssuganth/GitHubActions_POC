using System.Collections.Generic;
using System.Net;

namespace SampleApp.Data
{
    public class ResponseModel<T>
    {
        public HttpStatusCode Status { get; set; }

        public T Result { get; set; }

        public bool IsSuccessful { get; set; }

        public List<ErrorResponse> Errors { get; set; }
    }

    public class ErrorResponse
    {
        public int Reason { get; set; }
        public string Message { get; set; }
    }
}