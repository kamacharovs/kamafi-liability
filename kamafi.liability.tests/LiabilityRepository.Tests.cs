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
    public class LiabilityRepositoryTests : LiabilityBaseTests<Liability, LiabilityDto>
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
            Assert.NotNull(liabilityType.Description);

            var nullDefaults = new string[]
            {
                LiabilityTypes.Base,
                LiabilityTypes.Other
            };

            if (!nullDefaults.Contains(liabilityType.Name))
            {
                Assert.NotNull(liabilityType.DefaultInterest);
                Assert.True(liabilityType.DefaultInterest > 0);
                Assert.NotNull(liabilityType.DefaultOriginalTerm);
                Assert.True(liabilityType.DefaultOriginalTerm > 0);
                Assert.NotNull(liabilityType.DefaultRemainingTerm);
                Assert.True(liabilityType.DefaultRemainingTerm > 0);
            }
            else
            {
                Assert.Null(liabilityType.DefaultInterest);
                Assert.Null(liabilityType.DefaultOriginalTerm);
                Assert.Null(liabilityType.DefaultRemainingTerm);
            }
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

            AssertLiability(
                 liability as Liability,
                 userId);
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityIdUserId), MemberType = typeof(Helper))]
        public async Task GetAsync_ById_IsSuccessful(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var liability = await repo.GetAsync(id);

            AssertLiability(
                liability as Liability,
                userId);
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityIdUserId), MemberType = typeof(Helper))]
        public async Task GetAsync_ById_NotFound_ThrowsNotFound(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();

            await Assert.ThrowsAsync<core.data.KamafiNotFoundException>(() => repo.GetAsync(id * 1000));
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
            Assert.NotNull(liabilityType.Description);
            Assert.NotNull(liabilityType.DefaultInterest);
            Assert.True(liabilityType.DefaultInterest > 0);
            Assert.NotNull(liabilityType.DefaultOriginalTerm);
            Assert.True(liabilityType.DefaultOriginalTerm > 0);
            Assert.NotNull(liabilityType.DefaultRemainingTerm);
            Assert.True(liabilityType.DefaultRemainingTerm > 0);

            Assert.Equal(dto.Name, liabilityType.Name);
            Assert.Equal(dto.Description, liabilityType.Description);
            Assert.Equal(dto.DefaultInterest, liabilityType.DefaultInterest);
            Assert.Equal(dto.DefaultOriginalTerm, liabilityType.DefaultOriginalTerm);
            Assert.Equal(dto.DefaultRemainingTerm, liabilityType.DefaultRemainingTerm);
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

        [Theory]
        [MemberData(nameof(Helper.LiabilityUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_IsSuccessful(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var dto = Helper.RandomLiabilityDto();

            var liability = await repo.AddAsync(dto) as Liability;

            AssertLiability(
                liability,
                userId);
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityUserId), MemberType = typeof(Helper))]
        public async Task AddAsync_ValidationError_ThrowsValidationException(int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var dto = Helper.RandomLiabilityDto();

            dto.Name = null;
            dto.TypeName = null;
            dto.Value = -1;

            await Assert.ThrowsAsync<ValidationException>(() => repo.AddAsync(dto));
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityIdUserId), MemberType = typeof(Helper))]
        public async Task UpdateAsync_IsSuccessful(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var dto = Helper.RandomLiabilityDto();

            var liabilityInDb = (await repo.GetAsync())
                .FirstOrDefault(x => x.Id == id);

            Assert.NotNull(liabilityInDb);

            var liability = await repo.UpdateAsync(liabilityInDb.Id, dto) as Liability;

            AssertUpdateLiability(
                liability,
                liabilityInDb as Liability,
                userId);
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityIdUserId), MemberType = typeof(Helper))]
        public async Task UpdateAsync_ValidationError_ThrowsValidationException(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();
            var dto = new LiabilityDto();

            dto.Value = -1;

            await Assert.ThrowsAsync<ValidationException>(() => repo.UpdateAsync(id, dto));
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityIdUserId), MemberType = typeof(Helper))]
        public async Task DeleteAsync_IsSuccessfuL(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();

            await repo.DeleteAsync(id);

            await Assert.ThrowsAsync<core.data.KamafiNotFoundException>(() => repo.GetAsync(id));
        }

        [Theory]
        [MemberData(nameof(Helper.LiabilityIdUserId), MemberType = typeof(Helper))]
        public async Task DeleteAsync_NotFound_ThrowsNotFound(int id, int userId)
        {
            var repo = new ServiceHelper() { UserId = userId }.GetRequiredService<ILiabilityRepository>();

            await Assert.ThrowsAsync<core.data.KamafiNotFoundException>(() => repo.DeleteAsync(id * 1000));
        }
    }
}
