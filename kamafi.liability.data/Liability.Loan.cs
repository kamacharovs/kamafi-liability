using System;
using System.ComponentModel.DataAnnotations;

namespace kamafi.liability.data
{
    public class Loan : Liability
    {
        public string LoanType { get; set; }
        public bool? ShortTerm { get; set; }
        public bool? LongTerm { get; set; }
    }

    public class LoanDto : LiabilityDto
    {
        [MaxLength(100)]
        public string LoanType { get; set; }
    }
}