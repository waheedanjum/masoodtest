using Microsoft.OpenApi.Models;
using Repository.DBContext;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using PsIntegrations.Services;
using PsIntegrations.Interfaces;
using Microsoft.Extensions.Options;

namespace PsIntegrations
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers();
            services.AddDbContext<ParagonIntegrationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PSIntegrationDBConnection")));
            services.AddTransient<IJwtService, JwtService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Paragon API Integration",
                    Description = "Authentication and Authorization in ASP.NET Core 6.0 with JWT and Swagger"
                });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PSIntegrations v1"));
            app.UseSwagger();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
