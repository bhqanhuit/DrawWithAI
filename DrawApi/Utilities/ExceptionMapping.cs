using System.ComponentModel.DataAnnotations;
using DrawApi.Exceptions;

namespace DrawApi.Utilities
{
    public static class ExceptionMapping
    {
        public static readonly Dictionary<Type, (int StatusCode, string Message)> ExceptionMap = new()
        {
            { typeof(BadRequestException), (400, "Bad Request") },
            { typeof(AIServiceException), (503, "AI Service Unavailable") },
            { typeof(DriveServiceException), (503, "Drive Service Unavailable") },
            { typeof(UnexpectedException), (500, "Something Unexpected Happens") }
            // Add more exceptions as needed
        };
    }
}