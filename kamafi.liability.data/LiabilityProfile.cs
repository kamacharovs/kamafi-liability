using System;

using AutoMapper;

namespace kamafi.liability.data
{
    public class LiabilityProfile : Profile
    {
        public LiabilityProfile()
        {
            /*
             * Liability
             */
            CreateMap<LiabilityDto, Liability>()
                .ForMember(x => x.Name, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.Name)))
                .ForMember(x => x.TypeName, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.TypeName)))
                .ForMember(x => x.Value, o => o.Condition(s => s.Value.HasValue))
                .ForMember(x => x.MonthlyPayment, o => o.Condition(s => s.MonthlyPayment.HasValue))
                .ForMember(x => x.Years, o => o.Condition(s => s.Years.HasValue));

            /*
             * Vehicle
             */
            CreateMap<VehicleDto, Vehicle>()
                .ForMember(x => x.Name, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.Name)))
                .ForMember(x => x.TypeName, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.TypeName)))
                .ForMember(x => x.Value, o => o.Condition(s => s.Value.HasValue))
                .ForMember(x => x.DownPayment, o => o.Condition(s => s.DownPayment.HasValue));

            /*
             * Loan
             */
            CreateMap<LoanDto, Loan>()
                .ForMember(x => x.Name, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.Name)))
                .ForMember(x => x.TypeName, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.TypeName)))
                .ForMember(x => x.Value, o => o.Condition(s => s.Value.HasValue))
                .ForMember(x => x.LoanType, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.LoanType)))
                .ForMember(x => x.Interest, o => o.Condition(s => s.Interest.HasValue));
        }
    }
}