using System;
using System.ComponentModel.DataAnnotations;

namespace kamafi.liability.data
{
    public class Loan : Liability
    {
        public string LoanType { get; set; }
        public decimal Interest { get; set; }
    }

    public class LoanDto : LiabilityDto
    {
        [MaxLength(100)]
        public string LoanType { get; set; }

        public decimal? Interest { get; set; }
    }
}