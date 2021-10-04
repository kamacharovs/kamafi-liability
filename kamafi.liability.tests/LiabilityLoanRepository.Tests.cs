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
    public class LiabilityLoanRepositoryTests
    {
        [Theory]
        [MemberData(nameof(Helper.LoanUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_IsSuccessful(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILoanRepository>();
            var dto = Helper.RandomLoanDto();

            var liability = await repo.AddAsync(dto);

            Assert.NotNull(liability);
            Assert.NotNull(liability.Name);
            Assert.NotNull(liability.Type);
            Assert.NotNull(liability.TypeName);
            Assert.True(liability.Value > 0);
            Assert.True(liability.MonthlyPayment > 0);
            Assert.True(liability.Years > 0);
            Assert.Equal(userId, liability.UserId);
            Assert.False(liability.IsDeleted);
            Assert.NotNull(liability.LoanType);
            Assert.True(liability.Interest > 0);
        }

        [Theory]
        [MemberData(nameof(Helper.LoanUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_ValidationError_ThrowsValidationException(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILoanRepository>();
            var dto = Helper.RandomLoanDto();

            dto.Name = null;
            dto.TypeName = null;
            dto.Value = -1;

            await Assert.ThrowsAsync<ValidationException>(() => repo.AddAsync(dto));
        }

        [Theory]
        [MemberData(nameof(Helper.LoanIdUserId), MemberType = typeof(Helper))]
        public async Task UpdateAsync_IsSuccessful(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILoanRepository>();
            var dto = Helper.RandomLoanDto();

            var liability = await repo.UpdateAsync(id, dto);

            Assert.NotNull(liability);
            Assert.NotNull(liability.Name);
            Assert.NotNull(liability.Type);
            Assert.NotNull(liability.TypeName);
            Assert.True(liability.Value > 0);
            Assert.True(liability.MonthlyPayment > 0);
            Assert.True(liability.Years > 0);
            Assert.Equal(userId, liability.UserId);
            Assert.False(liability.IsDeleted);
            Assert.NotNull(liability.LoanType);
            Assert.True(liability.Interest > 0);
        }
    }
}
