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

        T IAbstractHandler<T, TDto>.Handle(T liability)
        {
            if (liability is null) throw new ArgumentNullException(nameof(liability));

            return OnHandle(liability);
        }

        T IAbstractHandler<T, TDto>.HandleUpdate(TDto dto, T liability)
        {
            if (liability is null) throw new ArgumentNullException(nameof(liability));

            return OnHandleUpdate(dto, liability);
        }

        protected abstract TDto OnBeforeHandle(TDto dto);
        protected abstract T OnHandle(T liability);
        protected abstract T OnHandleUpdate(TDto dto, T liability);
    }
}
