using System.Net;
using LoadExpressApi.Application.Common;
using LoadExpressApi.Application.Common.Interface;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace LoadExpressApi.Persistence.Logging.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUser _currentUser;

    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(
        ICurrentUser currentUser,
        ISerializerService jsonSerializer)
    {
        _currentUser = currentUser;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
            string userId = _currentUser.GetUserId();
            if (!string.IsNullOrEmpty(userId)) LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("UserEmail", email);
            string errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", SensitiveInfo.MaskSensitiveInfo(exception.StackTrace));
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = $"Provide the ErrorId {exception.StackTrace} to the support team for further analysis."
            };



            switch (exception)
            {

                case KeyNotFoundException:
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode} and Error Id {errorId}.");
            var response = context.Response;
            errorResult.Messages = new List<string>()
            {
                "Request an access token using a refresh token."
            };
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}