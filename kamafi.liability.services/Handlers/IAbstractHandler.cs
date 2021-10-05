using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kamafi.liability.data;

namespace kamafi.liability.services.handlers
{
    public interface IAbstractHandler<T, TDto>
        where T : Liability
        where TDto : LiabilityDto
    {
        public TDto BeforeHandle(TDto dto);
        public T Handle(T liability);

        public T HandleUpdate(TDto dto, T liability);
    }
}
