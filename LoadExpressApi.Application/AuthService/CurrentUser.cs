using LoadExpressApi.Application.Common.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Application.AuthService
{

    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public string? FullName => $"{_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name)} {_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Surname)}";

        public IEnumerable<Claim>? GetUserClaims() => _httpContextAccessor?.HttpContext?.User.Claims;

        public string? GetUserEmail() =>
            IsAuthenticated()
                ? _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
                : string.Empty;


        public string? GetUserPhoneNumber() =>
            IsAuthenticated()
                ? _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.MobilePhone) ?? null
                : string.Empty;


        public string GetUserId()
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            if (userId is null)
                return string.Empty;

            return IsAuthenticated() ? userId : string.Empty;
        }

        public bool IsAuthenticated() =>
           _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
