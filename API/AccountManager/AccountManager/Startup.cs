using System.Collections.Generic;
using System.Text;
using AccountManager.Buisness.Helpers;
using AccountManager.Buisness.Implements;
using AccountManager.Buisness.Interfaces;
using AccountManager.Data;
using AccountManager.Data.Implements;
using AccountManager.Data.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace AccountManager
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
            services.AddCors();
            services.AddDbContext<AccountManagerContext>(x =>
                x.UseSqlServer(Configuration.GetSection("ConnectionString").Value));
            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Account manager api", Version = "v1"});

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] {} }
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });

            services.AddAutoMapper();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                });
                //.AddFacebook(options =>
                //{
                //    options.AppId = Configuration["auth:facboo"];
                //    options.AppSecret = Configuration[""];
                //})
                //.AddGoogle(options =>
                //{
                //    options.ClientId = Configuration["auth:google:clientid"];
                //    options.ClientSecret = Configuration["auth:google:clientsecret"];
                //})
                //.AddTwitter(options =>
                //{
                //    options.ConsumerKey = Configuration["auth:twitter:consumerkey"];
                //    options.ConsumerSecret = Configuration["auth:twitter:consumersecret"];
                //});

            services.AddScoped<DbContext, AccountManagerContext>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account manager api v1");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseDeveloperExceptionPage();

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            var options = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(options);

            app.UseMvc();
        }
    }
}
