using System;

namespace kamafi.liability.data
{
    public interface ILiabilityType
    {
        string Name { get; set; }
        Guid PublicKey { get; set; }
    }
}