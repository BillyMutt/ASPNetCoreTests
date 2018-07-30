using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreDeepDive
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
            //  Add custom middleware to dependency injection container.
            services.AddJohan();

            //  Add other middleware.
            services.AddMvc();

            //  Add SignalR.
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //  Add custom middleware to the middleware pipeline.
            app.UseJohan(new JohanMiddlewareOptions("JohanLog.txt"));

            //  Add SignalR to the pipeline.
            app.UseSignalR(route =>
            {
                route.MapHub<JohanHub>("/hub");
            });

            //  Add other middleware to the pipeline.
            app.UseMvc();
        }
    }
}
