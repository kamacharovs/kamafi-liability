using System;
using System.Diagnostics.CodeAnalysis;

using kamafi.liability.data;

namespace kamafi.liability.services.handlers
{
    [ExcludeFromCodeCoverage]
    public abstract class AbstractHandler<T, TDto> : IAbstractHandler<T, TDto>
        where T : Liability
        where TDto : LiabilityDto
    {
        TDto IAbstractHandler<T, TDto>.BeforeHandle(TDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(liability));

            return OnBeforeHandle(dto);
        }

        T IAbstractHandler<T, TDto>.HandleAdd(T liability)
        {
            if (liability is null) throw new ArgumentNullException(nameof(liability));

            liability.EstimateMonthlyPayment();

            return OnHandleAdd(liability);
        }

        T IAbstractHandler<T, TDto>.HandleUpdate(TDto dto, T liability)
        {
            if (liability is null) throw new ArgumentNullException(nameof(liability));
            else if (dto is null) throw new ArgumentNullException(nameof(dto));

            // Re-calculate MonthlyPaymentEstimate based on differences
            if (dto.RemainingTerm.HasValue && dto.RemainingTerm != liability.RemainingTerm
                || dto.Value.HasValue && dto.Value != liability.Value
                || dto.Interest.HasValue && dto.Interest != liability.Interest)
            {
                liability.MonthlyPaymentEstimate = ExtensionMethods.CalculatePayment(
                    dto.RemainingTerm.HasValue ? dto.RemainingTerm : liability.RemainingTerm,
                    dto.Value.HasValue ? (double)dto.Value : (double)liability.Value,
                    dto.Interest.HasValue ? (double)dto.Interest : (double)liability.Interest);
            }

            return OnHandleUpdate(dto, liability);
        }

        protected abstract TDto OnBeforeHandle(TDto dto);
        protected abstract T OnHandleAdd(T liability);
        protected abstract T OnHandleUpdate(TDto dto, T liability);
    }
}
