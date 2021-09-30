using System;

namespace kamafi.liability.data
{
    public interface ILiability
    {
        int Id { get; set; }
        Guid PublicKey { get; set; }
        string Name { get; set; }
        string TypeName { get; set; }
        LiabilityType Type { get; set; }
        decimal Value { get; set; }
        decimal? MonthlyPayment { get; set; }
        int? Years { get; set; }
        DateTime Created { get; set; }
        int UserId { get; set; }
        bool IsDeleted { get; set; }
    }
}