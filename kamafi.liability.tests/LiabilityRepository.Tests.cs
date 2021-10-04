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
    public class LiabilityRepositoryTests
    {
        [Theory]
        [MemberData(nameof(Helper.LiabilityUserId), MemberType = typeof(Helper))]
        public async Task GetTypesAsync_IsSuccessful(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var liabilityTypes = await repo.GetTypesAsync();

            Assert.NotNull(liabilityTypes);
            Assert.NotEmpty(liabilityTypes);

            var liabilityType = liabilityTypes.FirstOrDefault();

            Assert.NotNull(liabilityType);
            Assert.NotNull(liabilityType.Name);
            Assert.NotEqual(Guid.Empty, liabilityType.PublicKey);
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityUserId), MemberType = typeof(Helper))]
        public async Task GetAsync_IsSuccessful(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var liabilities = await repo.GetAsync();

            Assert.NotNull(liabilities);
            Assert.NotEmpty(liabilities);

            var liability = liabilities.FirstOrDefault();

            Assert.NotNull(liability);
            Assert.NotNull(liability.Name);
            Assert.NotNull(liability.Type);
            Assert.NotNull(liability.TypeName);
            Assert.True(liability.Value > 0);
            Assert.True(liability.MonthlyPayment > 0);
            Assert.True(liability.Years > 0);
            Assert.Equal(userId, liability.UserId);
            Assert.False(liability.IsDeleted);
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_Type_IsSuccessful(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var dto = Helper.RandomLiabilityTypeDto();

            var liabilityType = await repo.AddAsync(dto);

            Assert.NotNull(liabilityType);
            Assert.NotNull(liabilityType.Name);
            Assert.NotEqual(Guid.Empty, liabilityType.PublicKey);
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityType), MemberType = typeof(Helper))]
        public async Task AddAsync_Type_AlreadyExists_ThrowsBadRequest(string name)
        {
            var repo = new ServiceHelper().GetRequiredService<ILiabilityRepository>();
            var dto = new LiabilityTypeDto { Name = name };

            var exception = await Assert.ThrowsAsync<core.data.KamafiFriendlyException>(() => repo.AddAsync(dto));

            Assert.NotNull(exception);
            Assert.Equal(400, exception.StatusCode);
            Assert.Contains("already exists", exception.Message, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}