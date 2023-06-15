using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Make_a_Drop.Application.Common.Email;
using Make_a_Drop.Application.MappingProfiles;
using Make_a_Drop.Application.Services;
using Make_a_Drop.Application.Services.Impl;
using Make_a_Drop.Shared.Services;
using Make_a_Drop.Shared.Services.Impl;
using Make_a_Drop.Application.Helpers;

namespace Make_a_Drop.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddServices(env);

        services.RegisterAutoMapper();

        return services;
    }

    private static void AddServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IDropService, DropService>();
        services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
        services.AddScoped<IDropFileService, DropFileService>();
        services.AddScoped<ICollaborationService, CollaborationService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddSingleton<IHostedService, DropCleanupJob>();



        if (env.IsDevelopment())
            services.AddScoped<IEmailService, EmailService>();
        else
            services.AddScoped<IEmailService, EmailService>();
    }

    private static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(IMappingProfilesMarker));
    }

    public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("SmtpSettings").Get<SmtpSettings>());
    }
}
