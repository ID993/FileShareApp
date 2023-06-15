using Make_a_Drop.DataAccess;
using Make_a_Drop.Application;
using Make_a_Drop.Application.Validators;
using Make_a_Drop.MVC.Filters;
using FluentValidation.AspNetCore;
using Make_a_Drop.MVC.Middleware;


namespace Make_a_Drop.MVC
{
    public class Startup
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddControllersWithViews();

            services.AddControllers(
                config => config.Filters.Add(typeof(ValidateModelAttribute))
            )
            .AddFluentValidation(
                options => options.RegisterValidatorsFromAssemblyContaining<IValidationsMarker>()
            );

            services.AddDataAccess(_configuration)
            .AddApplication(_env);

            services.AddRazorPages();

            services.AddHttpContextAccessor();

           
            services.AddEmailConfiguration(_configuration);


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseHttpsRedirection();
           
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<PerformanceMiddleware>();

            app.UseMiddleware<TransactionMiddleware>();

            app.UseMiddleware<ExceptionHandlingMiddleware>();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

       
        }
    }

}

