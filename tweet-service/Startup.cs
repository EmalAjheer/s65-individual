using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tweet_service.Models;
using tweet_service.Services;
using tweet_service.Interfaces;
using tweet_service.KafkaConsumers;

namespace tweet_service
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
            services.AddScoped<ITweetService, TweetService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "tweet_service", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicyAllHosts", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            var connectionString = Configuration["Data:DbContext:ConnectionString"];
            //"Host=localhost; port=5432; Database=tweetdb; Username=postgres; Password=admin"
            //"Server=s65tweet; port=5432; Database=tweetdb; Username=postgres; Password=admin"
            services.AddDbContext<TweetContext>(options => options.UseNpgsql(connectionString));

            services.AddSingleton<IHostedService, KafkaConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TweetContext tweetContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "tweet_service v1"));
            }

            app.UseCors("CorsPolicyAllHosts");

            //tweetContext.Database.EnsureCreated();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
