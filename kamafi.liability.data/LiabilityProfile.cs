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
                    o.MapFrom(s => s.Name.ToLowerInvariant());
                });

            /*
             * Liability
             */
            CreateMap<LiabilityDto, Liability>()
                .Include<VehicleDto, Vehicle>()
                .Include<LoanDto, Loan>()
                .ForMember(x => x.Name, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.Name)))
                .ForMember(x => x.TypeName, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.TypeName)))
                .ForMember(x => x.Value, o => o.Condition(s => s.Value.HasValue))
                .ForMember(x => x.MonthlyPayment, o => o.Condition(s => s.MonthlyPayment.HasValue))
                .ForMember(x => x.OriginalTerm, o => o.Condition(s => s.OriginalTerm.HasValue))
                .ForMember(x => x.RemainingTerm, o => o.Condition(s => s.RemainingTerm.HasValue))
                .ForMember(x => x.Interest, o => o.Condition(s => s.Interest.HasValue))
                .ForMember(x => x.AdditionalPayments, o => o.Condition(s => s.AdditionalPayments.HasValue));

            /*
             * Vehicle
             */
            CreateMap<VehicleDto, Vehicle>()
                .ForMember(x => x.DownPayment, o => o.Condition(s => s.DownPayment.HasValue));

            /*
             * Loan
             */
            CreateMap<LoanDto, Loan>()
                .ForMember(x => x.LoanType, o => o.Condition(s => !string.IsNullOrWhiteSpace(s.LoanType)))
                .ForMember(x => x.ShortTerm, o =>
                {
                    o.PreCondition(s => s.OriginalTerm.HasValue);
                    o.MapFrom(s => s.OriginalTerm < 180);
                })
                .ForMember(x => x.LongTerm, o =>
                {
                    o.PreCondition(s => s.OriginalTerm.HasValue);
                    o.MapFrom(s => s.OriginalTerm >= 180);
                });
        }
    }
}