using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Make_a_Drop.MVC
{
    public static class ApiDependencyInjection
    {
        //public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var secretKey = configuration.GetValue<string>("JwtConfiguration:SecretKey");

        //    var key = Encoding.ASCII.GetBytes(secretKey);

        //    services.AddAuthentication(x =>
        //    {
        //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //        .AddJwtBearer(x =>
        //        {
        //            x.RequireHttpsMetadata = false;
        //            x.SaveToken = true;
        //            x.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = new SymmetricSecurityKey(key),
        //                ValidateIssuer = false,
        //                ValidateAudience = false
        //            };
        //        });
        //}
    }
}

