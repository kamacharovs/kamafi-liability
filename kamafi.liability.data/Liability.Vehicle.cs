using System;

namespace kamafi.liability.data
{
    public class Vehicle : Liability
    {
        public decimal DownPayment { get; set; } = 0;
        public decimal Interest { get; set; }
    }

    public class VehicleDto : LiabilityDto
    {
        public decimal? DownPayment { get; set; }
        public decimal? Interest { get; set; }
    }
}