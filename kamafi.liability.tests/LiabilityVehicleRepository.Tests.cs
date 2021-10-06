using System;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using FluentValidation;

using kamafi.liability.services;
using kamafi.liability.data;

namespace kamafi.liability.tests
{
    [Trait(Helper.Category, Helper.UnitTest)]
    public class LiabilityVehicleRepositoryTests : LiabilityBaseTests<Vehicle, VehicleDto>
    {
        [Theory]
        [MemberData(nameof(Helper.VehicleUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_IsSuccessful(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<IVehicleRepository>();
            var dto = Helper.RandomVehicleDto();

            var liability = await repo.AddAsync(dto);

            AssertLiability(
                liability, 
                userId);

            Assert.True(liability.DownPayment > 0);
            Assert.Equal(dto.DownPayment, liability.DownPayment);
        }

        [Theory]
        [MemberData(nameof(Helper.VehicleUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_ValidationError_ThrowsValidationException(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<IVehicleRepository>();
            var dto = Helper.RandomVehicleDto();

            dto.Name = null;
            dto.TypeName = null;
            dto.Value = -1;

            await Assert.ThrowsAsync<ValidationException>(() => repo.AddAsync(dto));
        }

        [Theory]
        [MemberData(nameof(Helper.VehicleIdUserId), MemberType = typeof(Helper))]
        public async Task UpdateAsync_IsSuccessful(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<IVehicleRepository>();
            var dto = Helper.RandomVehicleDto();

            var liability = await repo.UpdateAsync(id, dto);

            AssertLiability(
                liability,
                userId);

            Assert.True(liability.DownPayment > 0);
            Assert.Equal(dto.DownPayment, liability.DownPayment);
        }
    }
}
