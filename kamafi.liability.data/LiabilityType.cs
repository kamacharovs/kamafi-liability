using System;

namespace kamafi.liability.data
{
    public class LiabilityType
    {
        public string Name { get; set; }
        public Guid PublicKey { get; set; } = Guid.NewGuid();
    }

    public class LiabilityTypeDto
    {
        public string Name { get; set; }
    }
}