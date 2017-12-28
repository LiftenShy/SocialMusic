using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialMusic.Buisness.Abstract;
using SocialMusic.API.Helper;
using SocialMusic.Buisness;
using SocialMusic.Data;
using SocialMusic.Data.Abstract;
using Swashbuckle.AspNetCore.Swagger;

namespace SocialMusic.API
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private MapperConfiguration _mapperConfiguration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowAnyOrigin()
                            .AllowCredentials();
                    });
            });

            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            });

            var connection = @"Server=localhost\SQLEXPRESS;Database=SocialMusic;Trusted_Connection=True;";

            services.AddDbContext<SocialMusicContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

            services.AddSingleton(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IUserProfileService, UserProfileService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SocialMusic API", Version = "v1" });
            });

            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowSpecificOrigin");

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialMusic API V1");
            });
        }
    }
}
