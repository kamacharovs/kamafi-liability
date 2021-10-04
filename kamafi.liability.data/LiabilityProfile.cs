using System;

using AutoMapper;

using kamafi.core.data;

namespace kamafi.liability.data
{
    public class LiabilityProfile : Profile
    {
        public LiabilityProfile()
        {
            /*
             * Liability type
             */
            CreateMap<LiabilityTypeDto, LiabilityType>()
                .ForMember(x => x.Name, o =>
                {
                    o.PreCondition(s => !string.IsNullOrWhiteSpace(s.Name));
                    o.MapFrom(s => s.Name.ToSnakeCase().ToLowerInvariant());
                });

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
                .ForMember(x => x.Interest, o => o.Condition(s => s.Interest.HasValue))
                .ForMember(x => x.ShortTerm, o =>
                {
                    o.PreCondition(s => s.Years.HasValue);
                    o.MapFrom(s => s.Years < 15);
                })
                .ForMember(x => x.LongTerm, o =>
                {
                    o.PreCondition(s => s.Years.HasValue);
                    o.MapFrom(s => s.Years >= 15);
                });
        }
    }
}