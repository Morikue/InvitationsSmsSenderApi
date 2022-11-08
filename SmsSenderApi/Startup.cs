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
using SmsSenderApi.Domain;
using SmsSenderApi.Domain.Models;
using SmsSenderApi.Repositories.EntityFramework;
using SmsSenderApi.Repositories.Interfaces;
using SmsSenderApi.Services.Implementation;
using SmsSenderApi.Services.Interfaces;
using SmsSenderApi.Validators.Implementation;
using SmsSenderApi.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmsSenderApi
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
            services.AddDbContext<SmsSenderDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SmsSenderConnection")));

            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IMessageRepository, EFMessageRepository>();

            services.AddSingleton<IValidator<List<string>>, InternationalFormatNumberValidator>();
            services.AddSingleton<IValidator<List<string>>, EmptyPhoneNumbersValidator>();
            services.AddSingleton<IValidator<List<string>>, PhoneNumbersAmountValidator>();
            services.AddSingleton<IValidator<List<string>>, DuplicationOfPhoneNumbersValidator>();
            services.AddSingleton<IValidator<string>, EmptyMessageValidator>();
            services.AddSingleton<IValidator<string>, GsmAndTransliteratedCyrillicValidator>();
            services.AddSingleton<IValidator<string>, MessageLengthValidator>();
            services.AddSingleton<IAllValidator<string>, MessageAllValidator>();
            services.AddSingleton<IAllValidator<List<string>>, PhoneNumbersAllValidator>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmsSenderApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmsSenderApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
