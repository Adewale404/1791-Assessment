using LoadExpressApi.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LoadExpressApi.Host.Settings
{
    public class SessionCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SessionCheckMiddleware> _logger;
        private readonly IHttpContextAccessor _httpContext;
        public SessionCheckMiddleware(RequestDelegate next, IServiceProvider serviceProvider, IHttpContextAccessor httpContext, ILogger<SessionCheckMiddleware> logger)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {

                var endpoint = context.GetEndpoint();
                var tokenString = context.Request.Headers.Authorization.ToString();
                var token = "";

                if (endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() != null && endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() == null && tokenString.Contains("Bearer "))
                {

                    token = tokenString.Split(" ")[1];
                    var path = context.Request.Path.Value;

                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jsonToken = handler.ReadToken(token);
                        var jwt_token = jsonToken as JwtSecurityToken;
                        IEnumerable<Claim> claims = jwt_token.Claims;

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


                            var sessionID = new List<Claim>(claims).Find(c => c.Type == "SessionId")?.Value;
                            var userID = new List<Claim>(claims).Find(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                            var user = _context.Users.Where(u => u.Id == userID).First();
/*
                            if (sessionID != user.CurrentSessionId || !user.IsSessionValid)
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                await context.Response.WriteAsync("Session is invalid.");
                                return;
                            }*/
                            
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("The Authorisation token could not be read.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(SessionCheckMiddleware)} action {ex}");
            }
            await _next(context);

        }
    }
}
