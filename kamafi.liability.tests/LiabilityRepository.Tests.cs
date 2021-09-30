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
    }
}
