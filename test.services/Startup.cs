using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using MassTransit;
using MetroBus;
using MetroBus.Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using test.services.Consumer;
using test.services.Service;

namespace test.services
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            MqConstant MqConstant = new MqConstant(Configuration);

            services.AddMetroBus(x => x.AddConsumer<TestActionConsumer>());

            services.AddSingleton<IBusControl>(provider => MetroBusInitializer.Instance
                .UseRabbitMq(MqConstant.RabbitMQUri, MqConstant.RabbitMQUserName, MqConstant.RabbitMQPassword)
                .RegisterConsumer<TestActionConsumer>("user.activation.queue", provider)
                .Build());

            services.AddHostedService<BusService>();
            services.AddScoped<ITestService, TestService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}