using System;
using System.ComponentModel.DataAnnotations;

namespace kamafi.liability.data
{
    public class LiabilityType : ILiabilityType
    {
        public string Name { get; set; }
        public Guid PublicKey { get; set; } = Guid.NewGuid();
        public string Description { get; set; }
        public decimal? DefaultInterest { get; set; }
        public int? DefaultOriginalTerm { get; set; }
        public int? DefaultRemainingTerm { get; set; }
    }

    public class LiabilityTypeDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public decimal? DefaultInterest { get; set; }
        public int? DefaultOriginalTerm { get; set; }
        public int? DefaultRemainingTerm { get; set; }
    }
}