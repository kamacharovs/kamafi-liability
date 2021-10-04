using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

using AutoMapper;
using FluentValidation;
using Moq;
using Bogus;
using RestSharp;

using kamafi.core.data;
using kamafi.liability.data;
using kamafi.liability.data.validators;
using kamafi.liability.services;

namespace kamafi.liability.tests
{
    [ExcludeFromCodeCoverage]
    public class ServiceHelper
    {
        public int? UserId { get; set; }
        public int? ClientId { get; set; }
        public Guid? PublicKey { get; set; }

        public T GetRequiredService<T>()
        {
            var provider = Services().BuildServiceProvider();

            provider.GetRequiredService<FakeDataManager>()
                .UseFakeContext();

            return provider.GetRequiredService<T>();
        }

        public ServiceCollection Services()
        {
            var services = new ServiceCollection();

            services.AddScoped<ILiabilityRepository, LiabilityRepository>()
                .AddScoped<IVehicleRepository, VehicleRepository>()
                .AddScoped<ILoanRepository, LoanRepository>()
                .AddScoped<FakeDataManager>();

            services.AddSingleton<IValidator<LiabilityDto>, LiabilityDtoValidator<LiabilityDto>>()
                .AddSingleton<IValidator<VehicleDto>, VehicleDtoValidator>()
                .AddSingleton<IValidator<LoanDto>, LoanDtoValidator>();

            services.AddScoped(x => GetMockTenant());
            services.AddSingleton(new MapperConfiguration(x =>
                {
                    x.AddProfile(new LiabilityProfile());
                })
                .CreateMapper());

            services.AddScoped<IConfiguration>(x =>
            {
                var configurationBuilder = new ConfigurationBuilder();

                return configurationBuilder.Build();
            });

            services.AddDbContext<LiabilityContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddLogging();
            services.AddFeatureManagement();
            services.AddHttpContextAccessor();

            return services;
        }

        public ITenant GetMockTenant()
        {
            var userId = UserId ?? 1;
            var clientId = ClientId ?? 1;
            var publicKey = PublicKey ?? Guid.NewGuid();

            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(core.data.Keys.Claim.UserId, userId.ToString()));
            ci.AddClaim(new Claim(core.data.Keys.Claim.ClientId, clientId.ToString()));
            ci.AddClaim(new Claim(core.data.Keys.Claim.PublicKey, publicKey.ToString()));
            var user = new ClaimsPrincipal(ci);

            var mockedHttpContext = new Mock<HttpContext>();
            mockedHttpContext.Setup(x => x.User).Returns(user);

            return new Tenant(mockedHttpContext.Object);
        }
    }

    [ExcludeFromCodeCoverage]
    public static class Helper
    {
        public const string Category = nameof(Category);
        public const string UnitTest = nameof(UnitTest);
        public const string IntegrationTest = nameof(IntegrationTest);

        #region UnitTests
        public static FakeDataManager _Fake
            => new ServiceHelper().GetRequiredService<FakeDataManager>() ?? throw new ArgumentNullException(nameof(FakeDataManager));


        public static IEnumerable<object[]> LiabilityType()
        {
            return _Fake.GetFakeLiabilityTypeData();
        }

        public static IEnumerable<object[]> LiabilityIdUserId()
        {
            return _Fake.GetFakeLiabilityData(
                id: true,
                userId: true);
        }

        public static IEnumerable<object[]> LiabilityUserId()
        {
            return _Fake.GetFakeLiabilityData(
                userId: true);
        }

        public static IEnumerable<object[]> VehicleIdUserId()
        {
            return _Fake.GetFakeVehicleData(
                id: true,
                userId: true);
        }

        public static IEnumerable<object[]> VehicleUserId()
        {
            return _Fake.GetFakeVehicleData(
                userId: true);
        }

        public static IEnumerable<object[]> LoanIdUserId()
        {
            return _Fake.GetFakeLoanData(
                id: true,
                userId: true);
        }

        public static IEnumerable<object[]> LoanUserId()
        {
            return _Fake.GetFakeLoanData(
                userId: true);
        }

        public static LiabilityTypeDto RandomLiabilityTypeDto()
        {
            return new Faker<LiabilityTypeDto>()
                .RuleFor(x => x.Name, f => f.Random.String2(10))
                .Generate();
        }

        public static LiabilityDto RandomLiabilityDto()
        {
            return FakerLiabilityDto<LiabilityDto>().Generate();
        }

        public static VehicleDto RandomVehicleDto()
        {
            return FakerLiabilityDto<VehicleDto>()
                .RuleFor(x => x.DownPayment, f => f.Random.Decimal(1000, 4000))
                .Generate();
        }

        public static LoanDto RandomLoanDto()
        {
            return FakerLiabilityDto<LoanDto>()
                .RuleFor(x => x.LoanType, f => f.Random.String2(5, 10))
                .RuleFor(x => x.Interest, f => f.Random.Decimal(0.01M, 1.5M))
                .Generate();
        }

        private static Faker<TDto> FakerLiabilityDto<TDto>()
            where TDto : LiabilityDto
        {
            return new Faker<TDto>()
                .RuleFor(x => x.Name, f => f.Random.String2(10))
                .RuleFor(x => x.TypeName, f => LiabilityTypes.Base)
                .RuleFor(x => x.Value, f => f.Random.Int(1000, 10000))
                .RuleFor(x => x.MonthlyPayment, f => f.Random.Decimal(50, 500))
                .RuleFor(x => x.Years, f => f.Random.Int(1, 5));
        }
        #endregion
    }
}
