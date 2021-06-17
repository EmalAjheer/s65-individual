using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trend_service.Consumers;
using trend_service.Interfaces;
using trend_service.Models;
using trend_service.Services;

namespace trend_service
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "trend_service", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicyAllHosts", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            var connectionString = Configuration["Data:DbContext:ConnectionString"];
            //"Host=localhost; port=5432; Database=trenddb; Username=postgres; Password=admin"
            //"Server=s65trend; port=5433; Database=trenddb; Username=postgres; Password=admin"
            services.AddDbContext<HashtagContext>(options => options.UseNpgsql(connectionString));
            //
            services.AddScoped<ITrendService, TrendService>();
            services.AddSingleton<IHostedService, KafkaConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HashtagContext hashtagContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "trend_service v1"));
            }

            hashtagContext.Database.EnsureCreated();
            // app.UseHttpsRedirection();
            app.UseCors("CorsPolicyAllHosts");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
