using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LoadExpressApi.Persistence.SecurityHeaders;

public static class Startup
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SecurityHeaderSettings)).Get<SecurityHeaderSettings>();

        if (settings?.Enable is true)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Response.HasStarted)
                {

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XXSSProtection))
                    {
                        context.Response.Headers.Append(HeaderNames.XXSSPROTECTION, settings.Headers.XXSSProtection);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.StrictTransportSecurity))
                    {
                        context.Response.Headers.Append(HeaderNames.STRICTTRANSPORTSECURITY, settings.Headers.StrictTransportSecurity);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XFrameOptions))
                    {
                        context.Response.Headers.Append(HeaderNames.XFRAMEOPTIONS, settings.Headers.XFrameOptions);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XContentTypeOptions))
                    {
                        context.Response.Headers.Append(HeaderNames.XCONTENTTYPEOPTIONS, settings.Headers.XContentTypeOptions);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.ContentSecurityPolicy))
                    {
                        context.Response.Headers.Append(HeaderNames.CONTENTSECURITYPOLICY, settings.Headers.ContentSecurityPolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.ReferrerPolicy))
                    {
                        context.Response.Headers.Append(HeaderNames.REFERRERPOLICY, settings.Headers.ReferrerPolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.FeaturePolicy))
                    {
                        context.Response.Headers.Append(HeaderNames.FEATUREPOLICY, settings.Headers.FeaturePolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.PermissionsPolicy))
                    {
                        context.Response.Headers.Append(HeaderNames.PERMISSIONSPOLICY, settings.Headers.PermissionsPolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.Server))
                    {
                        context.Response.Headers.Append(HeaderNames.SERVER, settings.Headers.Server);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XPoweredBy))
                    {
                        context.Response.Headers.Append(HeaderNames.XPOWEREDBY, settings.Headers.XPoweredBy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.SameSite))
                    {
                        context.Response.Headers.Append(HeaderNames.SAMESITE, settings.Headers.SameSite);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.XPermittedCrossDomainPolicies))
                    {
                        context.Response.Headers.Append(HeaderNames.XPERMITTEDCROSSDOMAINPOLICIES, settings.Headers.XPermittedCrossDomainPolicies);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.Headers.ExpectCT))
                    {
                        context.Response.Headers.Append(HeaderNames.EXPECTCT, settings.Headers.ExpectCT);
                    }
                }

                await next();
            });
        }

        return app;
    }
}