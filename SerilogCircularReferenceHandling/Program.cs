using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
namespace SerilogCircularReferenceHandling;

internal class Program
{
    private static void Main(string[] args)
    {
        var configuration = CreateConfiguration();
        ILogger logger = CreateLogger(configuration);
        var claimsPrincipal = CreateClaimsPrincipal();
        logger.Information(messageTemplate: "Blows up {@LogContext}", new { principal = claimsPrincipal });
    }

    private static IConfiguration CreateConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
    }
    private static Logger CreateLogger(IConfiguration configuration)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        return logger;
    }

    private static ClaimsPrincipal CreateClaimsPrincipal()
    {
        const string jwtToken = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ2ZXIiOjEsImp0aSI6IkFULnFGZ3ViZVJYTEk0M2JHVWd0cFZOYllJMUdwOEN0VEZ1NHZ4dEJyMXhuQU0iLCJpYXQiOjE3MjEzMjcyNTksImV4cCI6MTcyMTMzMDA0OCwiY2lkIjoiMG9hMXNqa293djhPZmFOdkowaDgiLCJzY3AiOiJpZGVudGl0eV91c2VyX2NyZWF0ZSIsInN1YiI6IjBvYTFzamtvd3Y4T2ZhTnZKMGg4IiwiaWRlbnRpdHlHcm91cHMiOlsiaWRlbnRpdHlfdXNlcl9jcmVhdGUiLCJpZGVudGl0eV91c2VyX2NvcnJlY3QiXSwibmJmIjowLCJpc3MiOiJodHRwczovL2R1bW15LmNvbSIsImF1ZCI6Imh0dHBzOi8vZHVtbXkuY29tIn0.";
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtTokenObj = tokenHandler.ReadJwtToken(jwtToken);
        var claimsIdentity = new ClaimsIdentity(jwtTokenObj.Claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        return claimsPrincipal;
    }
}
