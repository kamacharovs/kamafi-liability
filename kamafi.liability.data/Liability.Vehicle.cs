using System;

namespace kamafi.liability.data
{
    public class Vehicle : Liability
    {
        public decimal DownPayment { get; set; } = 0;
    }

    public class VehicleDto : LiabilityDto
    {
        public decimal? DownPayment { get; set; }
    }
}