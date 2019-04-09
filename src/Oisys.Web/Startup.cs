using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OisysNew.Configuration;
using OisysNew.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

namespace OisysNew
{
    public class Startup
    {
        private readonly IHostingEnvironment env;
        private const string DatabaseName = "OisysDb";

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            env = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OisysDbContext>(opt =>
            {
                opt.UseLazyLoadingProxies();

                if (env.IsDevelopment() || env.IsProduction())
                {
                    opt.UseSqlServer(Configuration.GetConnectionString(DatabaseName));
                }
                else if(string.Compare(env.EnvironmentName, "Heroku", true) == 0)
                {
                    opt.UseInMemoryDatabase(DatabaseName);
                }
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Authentication
            var authSection = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authSection);

            var authSettings = authSection.Get<AuthOptions>();
            var key = Encoding.UTF8.GetBytes(authSettings.Key);
            services
                .AddAuthentication(opt => 
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                    };
                });

            // App services
            services.AddAutoMapper();
            services.AddCoreServices();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            if (env.IsDevelopment())
            {
                services.AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new Info { Title = "Oisys API", Version = "v1", Description = "Order and Inventory System API" });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseSwagger()
                    .UseSwaggerUI(opt =>
                    {
                        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "OISYS API V1");
                        opt.RoutePrefix = "info";
                    });
            }
            else
            {
                app
                    .UseExceptionHandler("/Error")
                    .UseHsts();
            }

            app
                .UseStaticFiles()
                .UseSpaStaticFiles();

            app
                .UseAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action=Index}/{id?}");
                });

            app
                .UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                });
        }
    }
}
