using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniversalFrame.Core.Utils;
using UniversalFrame.Core.Web;
using UniversalFrame.Core.Web.Api.Builder;
using UniversalFrame.Core.Web.Session;

namespace RiskManager
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
            services.AddSession();
            services.AddMvc(o => o.EnableEndpointRouting = false).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddAshx(o => o.IsAsync = true).AddHttpContext();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(AllException);
            }

            app.UseSession();

            //app.UseAsSession();//可以暂时使用框架自带Session

            app.UseStaticFiles();

            app.UseAshx(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "Api/{controller=RiskServers}/{action=GetAsync}");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Risk}/{action=Login}/{id?}");
            });
        }
        public void AllException(HttpContext context, Exception exception)
        {
            context.Response.Write("An unknown error has occurred!");
            Log.Error("捕获全局异常：", exception);
        }

    }
}
