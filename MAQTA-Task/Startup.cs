using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MAQTA.DAL;
using MAQTA.DAL.Contracts;
using MAQTA.DAL.Entities;
using MAQTA.DAL.Repository;
using MAQTA.DAL.Seeder;
using System.Text;
using MAQTA.BL.Contracts;
using MAQTA.BL;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace MAQTA_Task
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddDbContext<MAQTADbContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<MAQTADbContext>();

            //return unauthorized instead of redirect
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.StatusCode = 401;
            //        return Task.CompletedTask;
            //    };
            //});


            //in case of jwt authintication
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("PasswordKey")))
                };
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IAccountsService), typeof(AccountsService));
            services.AddScoped(typeof(DataSeeder));

            services.AddSwaggerGen();
            // In production, the angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/client-app";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "MAQTA API Endpoint V1");
                 });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MAQTAD API Endpoint");
            //});

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                //spa.Options.SourcePath = "./";
                //Configure the timeout to x minutes to avoid "The Angular CLI process did not start listening for requests within the timeout period of 50 seconds." issue
                spa.Options.StartupTimeout = new TimeSpan(0, 0, 80);

                if (env.IsDevelopment())
                {

                    spa.UseAngularCliServer(npmScript: "start");
                }
                else
                {
                    spa.UseAngularCliServer(npmScript: "build");

                }
            });
        }
    }
}
