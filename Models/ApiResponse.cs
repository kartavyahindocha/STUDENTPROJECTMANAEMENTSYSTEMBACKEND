using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    /// <summary>
    /// Base non-generic API response wrapper containing HTTP status, success flag, messages, and errors.
    /// </summary>
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Error { get; set; }

        public List<string>? Errors { get; set; }

        public ApiResponse()
        {
        }

        #region Factory Methods (Non-Generic and Generic)

        /// <summary>
        /// 200 - OK (Non-Generic Response)
        /// </summary>
        public static ApiResponse Success(
            string message = "Success",
            int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Message = message,
                Error = null,
                Errors = null
            };
        }

        /// <summary>
        /// 200 - OK (Generic Data Response)
        /// </summary>
        public static ApiResponse<T> Success<T>(
            T data,
            string message = "Success",
            int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Error = null,
                Errors = null
            };
        }

        /// <summary>
        /// 201 - Created
        /// </summary>
        public static ApiResponse<T> Created<T>(
            T data,
            string message = "Record created successfully")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                Message = message,
                Data = data,
                Error = null,
                Errors = null
            };
        }

        /// <summary>
        /// 204 - No Content
        /// </summary>
        public static ApiResponse NoContent(
            string message = "No content")
        {
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status204NoContent,
                Message = message,
                Error = null,
                Errors = null
            };
        }

        /// <summary>
        /// Returns 200 when data exists, otherwise returns 404.
        /// </summary>
        public static ApiResponse<T> FromResponse<T>(
            T? data,
            string successMessage = "Data found",
            string errorMessage = "Record not found")
        {
            if (data == null)
            {
                return NotFound<T>(errorMessage);
            }

            return Success(data, successMessage);
        }

        /// <summary>
        /// 400 - Bad Request
        /// </summary>
        public static ApiResponse BadRequest(
            string errorMessage = "Bad request")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Bad Request",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 400 - Bad Request (Generic)
        /// </summary>
        public static ApiResponse<T> BadRequest<T>(
            string errorMessage = "Bad request")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Bad Request",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 400 - Bad Request with validation errors
        /// </summary>
        public static ApiResponse BadRequest(
            string message,
            IEnumerable<string> validationErrors)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message,
                Error = null,
                Errors = new List<string>(validationErrors)
            };
        }

        /// <summary>
        /// 401 - Unauthorized
        /// </summary>
        public static ApiResponse Unauthorized(
            string errorMessage = "Unauthorized access")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Unauthorized",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 401 - Unauthorized (Generic)
        /// </summary>
        public static ApiResponse<T> Unauthorized<T>(
            string errorMessage = "Unauthorized access")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Unauthorized",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 403 - Forbidden
        /// </summary>
        public static ApiResponse Forbidden(
            string errorMessage = "Access forbidden")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status403Forbidden,
                Message = "Forbidden",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 403 - Forbidden (Generic)
        /// </summary>
        public static ApiResponse<T> Forbidden<T>(
            string errorMessage = "Access forbidden")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status403Forbidden,
                Message = "Forbidden",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 404 - Not Found
        /// </summary>
        public static ApiResponse NotFound(
            string errorMessage = "Record not found")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not Found",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 404 - Not Found (Generic)
        /// </summary>
        public static ApiResponse<T> NotFound<T>(
            string errorMessage = "Record not found")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not Found",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 409 - Conflict
        /// </summary>
        public static ApiResponse Conflict(
            string errorMessage = "Conflict")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status409Conflict,
                Message = "Conflict",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 409 - Conflict (Generic)
        /// </summary>
        public static ApiResponse<T> Conflict<T>(
            string errorMessage = "Conflict")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status409Conflict,
                Message = "Conflict",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 422 - Unprocessable Entity
        /// </summary>
        public static ApiResponse UnprocessableEntity(
            string errorMessage = "Validation failed")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Message = "Validation Failed",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 422 - Unprocessable Entity with validation errors
        /// </summary>
        public static ApiResponse UnprocessableEntity(
            string message,
            IEnumerable<string> validationErrors)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Message = message,
                Error = null,
                Errors = new List<string>(validationErrors)
            };
        }

        /// <summary>
        /// 500 - Internal Server Error
        /// </summary>
        public static ApiResponse InternalServerError(
            string errorMessage = "Internal server error")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Internal Server Error",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 502 - Bad Gateway
        /// </summary>
        public static ApiResponse BadGateway(
            string errorMessage = "Bad gateway")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status502BadGateway,
                Message = "Bad Gateway",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// 503 - Service Unavailable
        /// </summary>
        public static ApiResponse ServiceUnavailable(
            string errorMessage = "Service unavailable")
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status503ServiceUnavailable,
                Message = "Service Unavailable",
                Error = errorMessage,
                Errors = null
            };
        }

        /// <summary>
        /// Creates a custom non-generic API response.
        /// </summary>
        public static ApiResponse Custom(
            bool isSuccess,
            int statusCode,
            string message,
            string? error = null,
            IEnumerable<string>? errors = null)
        {
            return new ApiResponse
            {
                IsSuccess = isSuccess,
                StatusCode = statusCode,
                Message = message,
                Error = error,
                Errors = errors != null
                    ? new List<string>(errors)
                    : null
            };
        }

        #endregion
    }

    /// <summary>
    /// Generic API response wrapper inheriting from non-generic <see cref="ApiResponse"/> and adding strongly-typed payload data.
    /// </summary>
    /// <typeparam name="T">Payload data type</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public ApiResponse() : base()
        {
        }

        #region Backward-Compatible Legacy Static Methods

        public static ApiResponse<T> SuccessResponse(
            T data,
            string message = "Success")
        {
            return ApiResponse.Success(data, message);
        }

        public static ApiResponse<T> CreatedResponse(
            T data,
            string message = "Record created successfully")
        {
            return ApiResponse.Created(data, message);
        }

        public static ApiResponse<T> NoContentResponse(
            string message = "No content")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status204NoContent,
                Message = message,
                Data = default,
                Error = null,
                Errors = null
            };
        }

        public static ApiResponse<T> FromResponse(
            T? data,
            string successMessage = "Data found",
            string errorMessage = "Record not found")
        {
            return ApiResponse.FromResponse(data, successMessage, errorMessage);
        }

        public static ApiResponse<T> BadRequestResponse(
            string errorMessage = "Bad request")
        {
            return ApiResponse.BadRequest<T>(errorMessage);
        }

        public static ApiResponse<T> BadRequestResponse(
            string message,
            IEnumerable<string> validationErrors)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message,
                Data = default,
                Error = null,
                Errors = new List<string>(validationErrors)
            };
        }

        public static ApiResponse<T> UnauthorizedResponse(
            string errorMessage = "Unauthorized access")
        {
            return ApiResponse.Unauthorized<T>(errorMessage);
        }

        public static ApiResponse<T> ForbiddenResponse(
            string errorMessage = "Access forbidden")
        {
            return ApiResponse.Forbidden<T>(errorMessage);
        }

        public static ApiResponse<T> NotFoundResponse(
            string errorMessage = "Record not found")
        {
            return ApiResponse.NotFound<T>(errorMessage);
        }

        public static ApiResponse<T> ConflictResponse(
            string errorMessage = "Conflict")
        {
            return ApiResponse.Conflict<T>(errorMessage);
        }

        public static ApiResponse<T> UnprocessableEntityResponse(
            string errorMessage = "Validation failed")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Message = "Validation Failed",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        public static ApiResponse<T> UnprocessableEntityResponse(
            string message,
            IEnumerable<string> validationErrors)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Message = message,
                Data = default,
                Error = null,
                Errors = new List<string>(validationErrors)
            };
        }

        public static ApiResponse<T> InternalServerErrorResponse(
            string errorMessage = "Internal server error")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Internal Server Error",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        public static ApiResponse<T> BadGatewayResponse(
            string errorMessage = "Bad gateway")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status502BadGateway,
                Message = "Bad Gateway",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        public static ApiResponse<T> ServiceUnavailableResponse(
            string errorMessage = "Service unavailable")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status503ServiceUnavailable,
                Message = "Service Unavailable",
                Data = default,
                Error = errorMessage,
                Errors = null
            };
        }

        public static ApiResponse<T> CustomResponse(
            bool isSuccess,
            int statusCode,
            string message,
            T? data = default,
            string? error = null,
            IEnumerable<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = isSuccess,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Error = error,
                Errors = errors != null
                    ? new List<string>(errors)
                    : null
            };
        }

        #endregion
    }
}