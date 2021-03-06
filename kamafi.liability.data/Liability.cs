using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace kamafi.liability.data
{
    public class Liability : ILiability
    {
        public int Id { get; set; }
        public Guid PublicKey { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string TypeName { get; set; }
        public LiabilityType Type { get; set; }
        public decimal Value { get; set; }
        public decimal MonthlyPayment { get; set; } = 0;
        public decimal? MonthlyPaymentEstimate { get; set; }
        public int OriginalTerm { get; set; }
        public int RemainingTerm { get; set; }
        public decimal Interest { get; set; }
        public decimal? AdditionalPayments { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class LiabilityDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [JsonIgnore]
        public string TypeName { get; set; }

        public decimal? Value { get; set; }
        public decimal? MonthlyPayment { get; set; }
        public int? OriginalTerm { get; set; }
        public int? RemainingTerm { get; set; }
        public decimal? Interest { get; set; }
        public decimal? AdditionalPayments { get; set; }
    }
}