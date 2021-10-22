using System;

namespace kamafi.liability.data
{
    public interface ILiabilityType
    {
        string Name { get; set; }
        Guid PublicKey { get; set; }
        decimal? DefaultInterest { get; set; }
        int? DefaultOriginalTerm { get; set; }
        int? DefaultRemainingTerm { get; set; }
    }
}