using System;
using System.IO;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using FluentValidation;

using kamafi.core.middleware;
using kamafi.liability.data;
using kamafi.liability.data.validators;
using kamafi.liability.services;

namespace kamafi.liability.core
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public readonly IConfiguration _config;
        public readonly IWebHostEnvironment _env;

        public Startup(
            IConfiguration config,
            IWebHostEnvironment env)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILiabilityRepository, LiabilityRepository>()
                .AddScoped<IVehicleRepository, VehicleRepository>()
                .AddScoped<ILoanRepository, LoanRepository>()
                .AddAutoMapper(typeof(LiabilityProfile).Assembly);

            services.AddSingleton<IValidator<LiabilityDto>, LiabilityDtoValidator<LiabilityDto>>()
                .AddSingleton<IValidator<VehicleDto>, VehicleDtoValidator>()
                .AddSingleton<IValidator<LoanDto>, LoanDtoValidator>();

            services.AddKamafiServices<LiabilityContext>(
                new kamafi.core.data.KamafiConfiguration
                {
                    Config = _config,
                    OpenApiName = Constants.ApiV1Full,
                    OpenApiVersion = Constants.ApiV1Full,
                    DefaultApiVersion = Constants.ApiV1,
                    XmlCommentsPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseKamafiServices(_config, _env);
        }
    }
}
