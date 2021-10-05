using System;

using kamafi.liability.services;
using kamafi.liability.data;

namespace kamafi.liability.services.handlers
{
    public class LiabilityHandler : AbstractHandler<Liability, LiabilityDto>
    {
        protected override LiabilityDto OnBeforeHandle(LiabilityDto dto)
        {
            dto.TypeName = LiabilityTypes.Base;

            return dto;
        }

        protected override Liability OnHandle(Liability liability)
        {
            return liability;
        }

        protected override Liability OnHandleUpdate(LiabilityDto dto, Liability liability)
        {
            return liability;
        }
    }
}
