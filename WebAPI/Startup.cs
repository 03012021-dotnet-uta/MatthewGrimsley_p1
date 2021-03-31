using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository;
using BusinessLogic;

namespace WebAPI
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
            // Register the configured Db context as a service
            services.AddDbContext<TheStore_DbContext>(options =>
            {
                options.UseSqlServer("Server=.\\SQLEXPRESS;Database=TheStore_Db;Trusted_Connection=True;");
            });

            // Register all business logic classes as services
            services.AddScoped<StoreMethods>();
            services.AddScoped<UserMethods>();

            // Register the repository class as a service
            services.AddScoped<TheStoreRepo>();

            // Register the WebAPI controllers as services
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            // Enables default text-only handlers for common error status codes
            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            // Redirects unknown paths to the index HTML
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^$", "index.html"));

            // Enables static files to be served
            // The default directory is {content root}/wwwroot
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
