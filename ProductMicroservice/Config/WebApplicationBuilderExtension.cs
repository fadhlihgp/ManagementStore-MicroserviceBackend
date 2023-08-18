using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ProductMicroservice.Config;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
    {
        var issuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer");
        var audience = builder.Configuration.GetValue<string>("JwtSettings:Audience");
        var expires = builder.Configuration.GetValue<int>("JwtSettings:ExpiresInMinutes");
        var keyJwt = builder.Configuration.GetValue<string>("JwtSettings:Key");
        var key = Encoding.UTF8.GetBytes(keyJwt);
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        return builder;
    }
}