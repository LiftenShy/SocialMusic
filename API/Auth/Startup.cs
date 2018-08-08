using Auth.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration confg, IHostingEnvironment env)
        {
            _environment = env;
            Configuration = confg;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
