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
                    Name = LiabilityTypes.Base,
                    Description = LiabilityTypes.Base,
                    DefaultInterest = null,
                    DefaultOriginalTerm = null,
                    DefaultRemainingTerm = null
                },
                new LiabilityType
                {
                    Name = LiabilityTypes.Vehicle,
                    Description = LiabilityTypes.Vehicle,
                    DefaultInterest = 6.25M,
                    DefaultOriginalTerm = 60,
                    DefaultRemainingTerm = 60
                },
                new LiabilityType
                {
                    Name = LiabilityTypes.Loan,
                    Description = LiabilityTypes.Loan,
                    DefaultInterest = 3,
                    DefaultOriginalTerm = 60,
                    DefaultRemainingTerm = 60
                },
                new LiabilityType
                {
                    Name = LiabilityTypes.Other,
                    Description = LiabilityTypes.Other,
                    DefaultInterest = null,
                    DefaultOriginalTerm = null,
                    DefaultRemainingTerm = null
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
                    MonthlyPaymentEstimate = 205.11M,
                    OriginalTerm = 36,
                    RemainingTerm = 36,
                    Interest = 3.25M,
                    AdditionalPayments = null,
                    Created = DateTime.UtcNow.AddDays(-5),
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
                    MonthlyPaymentEstimate = 275.95M,
                    OriginalTerm = 60,
                    RemainingTerm = 60,
                    Interest = 6.25M,
                    AdditionalPayments = null,
                    Created = DateTime.UtcNow.AddDays(-30),
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
                    MonthlyPaymentEstimate = 378M,
                    OriginalTerm = 120,
                    RemainingTerm = 120,
                    Interest = 10.25M,
                    AdditionalPayments = null,
                    Created = DateTime.UtcNow.AddDays(-15),
                    UserId = 1,
                    IsDeleted = false,
                    LoanType = "Personal"
                }
            };
        }

        public IEnumerable<object[]> GetFakeLiabilityTypeData()
        {
            var fakeLiabilityTypes = _context.LiabilityTypes
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
            return GetFakeLiabilityData<Liability>(
                id: id,
                userId: userId,
                isDeleted: isDeleted);
        }

        public IEnumerable<object[]> GetFakeVehicleData(
            bool id = false,
            bool userId = false,
            bool isDeleted = false)
        {
            return GetFakeLiabilityData<Vehicle>(
                id: id,
                userId: userId,
                isDeleted: isDeleted);
        }

        public IEnumerable<object[]> GetFakeLoanData(
            bool id = false,
            bool userId = false,
            bool isDeleted = false)
        {
            return GetFakeLiabilityData<Loan>(
                id: id,
                userId: userId,
                isDeleted: isDeleted);
        }

        private IEnumerable<object[]> GetFakeLiabilityData<T>(
            bool id = false,
            bool userId = false,
            bool isDeleted = false)
            where T : Liability
        {
            var fakeLiabilities = _context.Set<T>()
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
                foreach (var fakeLiabilityId in fakeLiabilities.Select(x => x.UserId).Distinct())
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
