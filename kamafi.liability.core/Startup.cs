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
using kamafi.liability.services.handlers;

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

            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;
            ValidatorOptions.Global.DisplayNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

            services.AddSingleton<IAbstractHandler<Liability, LiabilityDto>, LiabilityHandler>()
                .AddSingleton<IAbstractHandler<Loan, LoanDto>, LoanHandler>()
                .AddSingleton<IAbstractHandler<Vehicle, VehicleDto>, VehicleHandler>();

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
