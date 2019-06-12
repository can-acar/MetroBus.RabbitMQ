using Common;
using MetroBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using test.services.Service;

namespace test.api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            
            services.AddMvcCore()
                .AddApiExplorer();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddScoped<ITestService, TestService>();

            MqConstant MqConstant = new MqConstant(Configuration);

            services.AddSingleton(MetroBusInitializer.Instance.UseRabbitMq(MqConstant.RabbitMQUri, MqConstant.RabbitMQUserName, MqConstant.RabbitMQPassword).Build());

//            services.AddSingleton<ITracer>(serviceProvider =>
//            {
//                Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME", "User.API");
//                Environment.SetEnvironmentVariable("JAEGER_AGENT_HOST", "localhost");
//                Environment.SetEnvironmentVariable("JAEGER_AGENT_PORT", "6831");
//                Environment.SetEnvironmentVariable("JAEGER_SAMPLER_TYPE", "const");
//                
//                var loggerFactory = new LoggerFactory();
//
////                var config = Jaeger.Configuration.FromEnv(loggerFactory);
////                var tracer = config.GetTracer();
//
//               // GlobalTracer.Register(tracer);
//
//                return tracer;
//            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseHealthChecks("/health");

            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}