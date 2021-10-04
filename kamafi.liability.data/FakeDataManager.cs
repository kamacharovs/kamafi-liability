using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace kamafi.liability.data
{
    [ExcludeFromCodeCoverage]
    public class FakeDataManager
    {
        private readonly LiabilityContext _context;

        public FakeDataManager(LiabilityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void UseFakeContext()
        {
            _context.LiabilityTypes
                .AddRange(GetFakeLiabilityTypes());

            _context.Liabilities
                .AddRange(GetFakeLiabilities());

            _context.Vehicles
                .AddRange(GetFakeVehicles());

            _context.Loans
                .AddRange(GetFakeLoans());

            _context.SaveChanges();
        }

        public IEnumerable<LiabilityType> GetFakeLiabilityTypes()
        {
            return new List<LiabilityType>
            {
                new LiabilityType
                {
                    Name = LiabilityTypes.Base
                },
                new LiabilityType
                {
                    Name = LiabilityTypes.Vehicle
                },
                new LiabilityType
                {
                    Name = LiabilityTypes.Loan
                },
                new LiabilityType
                {
                    Name = LiabilityTypes.Other
                }
            };
        }

        public IEnumerable<Liability> GetFakeLiabilities()
        {
            return new List<Liability>
            {
                new Liability
                {
                    Id = 1,
                    Name = "Liability.Base",
                    TypeName = LiabilityTypes.Base,
                    Value = 1599.99M,
                    MonthlyPayment = 199.99M,
                    Years = 3,
                    UserId = 1,
                    IsDeleted = false
                }
            };
        }

        public IEnumerable<Vehicle> GetFakeVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle
                {
                    Id = 2,
                    Name = "Liability.Vehicle",
                    TypeName = LiabilityTypes.Vehicle,
                    Value = 3599.99M,
                    MonthlyPayment = 299.99M,
                    Years = 5,
                    UserId = 1,
                    IsDeleted = false,
                    DownPayment = 489.15M
                }
            };
        }

        public IEnumerable<Loan> GetFakeLoans()
        {
            return new List<Loan>
            {
                new Loan
                {
                    Id = 3,
                    Name = "Liability.Loan",
                    TypeName = LiabilityTypes.Loan,
                    Value = 3599.99M,
                    MonthlyPayment = 299.99M,
                    Years = 5,
                    UserId = 1,
                    IsDeleted = false,
                    LoanType = "Personal",
                    Interest = 1.25M
                }
            };
        }

        public IEnumerable<object[]> GetFakeLiabilityTypeData()
        {
            var fakeLiabilityTypes = GetFakeLiabilityTypes()
                .Select(x => x.Name);

            var toReturn = new List<object[]>();

            foreach (var fakeLiabilityTypeString in  fakeLiabilityTypes)
            {
                toReturn.Add(new object[]
                {
                    fakeLiabilityTypeString
                });
            }

            return toReturn;
        }

        public IEnumerable<object[]> GetFakeLiabilityData(
            bool id = false,
            bool userId = false,
            bool isDeleted = false)
        {
            var fakeLiabilities = GetFakeLiabilities()
                .Where(x => x.IsDeleted == isDeleted)
                .ToArray();

            var toReturn = new List<object[]>();

            if (id
                && userId)
            {
                foreach (var fakeLiability in fakeLiabilities)
                {
                    toReturn.Add(new object[]
                    {
                        fakeLiability.Id,
                        fakeLiability.UserId
                    });
                }
            }
            else if (userId)
            {
                foreach (var fakeLiabilityId in fakeLiabilities.Select(x => x.Id).Distinct())
                {
                    toReturn.Add(new object[]
                    {
                        fakeLiabilityId
                    });
                }
            }

            return toReturn;
        }
    }
}
