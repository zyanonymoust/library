using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Library.API.Security;

public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "ApiKey";
    public const string HeaderName = "X-API-Key";

    private readonly ApiKeyOptions _apiKeyOptions;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptions<ApiKeyOptions> apiKeyOptions)
        : base(options, logger, encoder)
    {
        _apiKeyOptions = apiKeyOptions.Value;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderName, out var values) || values.Count != 1)
        {
            Logger.LogWarning(
                "API key authentication rejected {Method} {Path}: key header missing or malformed",
                Request.Method,
                Request.Path);
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        byte[] providedKey = Encoding.UTF8.GetBytes(values[0] ?? string.Empty);
        byte[] configuredKey = Encoding.UTF8.GetBytes(_apiKeyOptions.ApiKey);

        if (providedKey.Length != configuredKey.Length ||
            !CryptographicOperations.FixedTimeEquals(providedKey, configuredKey))
        {
            Logger.LogWarning(
                "API key authentication rejected {Method} {Path}: key was invalid",
                Request.Method,
                Request.Path);
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
        }

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "api-client")],
            SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
