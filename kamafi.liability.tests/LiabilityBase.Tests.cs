using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using kamafi.liability.data;

namespace kamafi.liability.tests
{
    public abstract class LiabilityBaseTests<T, TDto>
        where T : Liability
        where TDto : LiabilityDto
    {
        public void AssertLiability(
            T liability,
            TDto dto,
            int userId)
        {
            // Assert Liability
            AssertLiability(
                liability,
                userId);

            // Assert Liability DTO values are equal to Liability values
            Assert.Equal(dto.Name, liability.Name);
            Assert.Equal(dto.TypeName, liability.TypeName);
            Assert.Equal(dto.Value, liability.Value);
            Assert.Equal(dto.MonthlyPayment, liability.MonthlyPayment);
            Assert.Equal(dto.OriginalTerm, liability.OriginalTerm);
            Assert.Equal(dto.RemainingTerm, liability.RemainingTerm);
            Assert.Equal(dto.Interest, liability.Interest);
            Assert.Equal(dto.AdditionalPayments, liability.AdditionalPayments);
        }

        public void AssertUpdateLiability(
            T liability,
            T liabilityInDb,
            int userId)
        {
            // Assert Liability in the DB values are equal to Liability values after an update
            Assert.NotEqual(liabilityInDb.Name, liability.Name);
            Assert.NotEqual(liabilityInDb.Value, liability.Value);
            Assert.NotEqual(liabilityInDb.MonthlyPayment, liability.MonthlyPayment);
            Assert.NotEqual(liabilityInDb.OriginalTerm, liability.OriginalTerm);
            Assert.NotEqual(liabilityInDb.RemainingTerm, liability.RemainingTerm);
            Assert.NotEqual(liabilityInDb.Interest, liability.Interest);
        }

        public void AssertLiability(
            T liability,
            int userId)
        {
            // Assert Liability
            Assert.NotNull(liability);
            Assert.NotEqual(liability.PublicKey, Guid.Empty);
            Assert.NotNull(liability.Name);
            Assert.NotNull(liability.Type);
            Assert.NotNull(liability.TypeName);
            Assert.True(liability.Value > 0);
            Assert.True(liability.MonthlyPayment > 0);
            Assert.True(liability.MonthlyPaymentEstimate > 0);
            Assert.True(liability.OriginalTerm > 0);
            Assert.True(liability.RemainingTerm > 0);
            Assert.True(liability.Interest > 0);
            Assert.Null(liability.AdditionalPayments);
            Assert.NotEqual(liability.Created, new DateTime());
            Assert.Equal(userId, liability.UserId);
            Assert.False(liability.IsDeleted);
        }
    }
}
