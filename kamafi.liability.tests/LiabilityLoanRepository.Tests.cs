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
    public class LiabilityLoanRepositoryTests : LiabilityBaseTests<Loan, LoanDto>
    {
        [Theory]
        [MemberData(nameof(Helper.LoanUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_IsSuccessful(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILoanRepository>();
            var dto = Helper.RandomLoanDto();

            var liability = await repo.AddAsync(dto);

            AssertLiability(
                liability, 
                userId);

            Assert.Equal(dto.LoanType, liability.LoanType);
            Assert.Equal(dto.Interest, liability.Interest);
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

            AssertLiability(
                liability,
                userId);

            Assert.NotNull(liability.LoanType);
            Assert.Equal(dto.LoanType, liability.LoanType);
            Assert.True(liability.ShortTerm);
            Assert.False(liability.LongTerm);
        }
    }
}
