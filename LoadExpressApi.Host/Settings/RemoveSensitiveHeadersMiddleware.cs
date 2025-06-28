namespace LoadExpressApi.Host.Settings
{

    public class RemoveSensitiveHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] sensitiveHeaders = { "Authorization", "Session-Token", "Custom-Header" };

        public RemoveSensitiveHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            foreach (var sensitiveHeader in sensitiveHeaders)
            {
                if (context.Request.Headers.ContainsKey(sensitiveHeader))
                {
                    context.Request.Headers.Remove(sensitiveHeader);
                }
            }

            await _next(context);
        }
    }
}
