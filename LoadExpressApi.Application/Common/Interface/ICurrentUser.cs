using LoadExpressApi.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LoadExpressApi.Application.Common.Interface
{
    public interface ICurrentUser : IScopedService
    {
        string? FullName { get; }
        string GetUserId();
        string? GetUserEmail();
        bool IsAuthenticated();
        string? GetUserPhoneNumber();
        IEnumerable<Claim>? GetUserClaims();
    }
}
