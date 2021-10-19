using System;

using Microsoft.EntityFrameworkCore;

using kamafi.core.data;

namespace kamafi.liability.data
{
    public class LiabilityContext : DbContext
    {
        public readonly ITenant Tenant;

        public virtual DbSet<LiabilityType> LiabilityTypes { get; set; }
        public virtual DbSet<Liability> Liabilities { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }

        public LiabilityContext(DbContextOptions<LiabilityContext> options, ITenant tenant)
            : base(options)
        {
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("liability");

            modelBuilder.Entity<LiabilityType>(e =>
            {
                e.ToTable(Keys.Entity.LiabilityType);

                e.HasKey(x => x.Name);

                e.Property(x => x.Name).HasSnakeCaseColumnName().HasMaxLength(100).IsRequired();
                e.Property(x => x.PublicKey).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.Description).HasSnakeCaseColumnName().HasMaxLength(500);
                e.Property(x => x.DefaultInterest).HasSnakeCaseColumnName();
                e.Property(x => x.DefaultOriginalTerm).HasSnakeCaseColumnName();
                e.Property(x => x.DefaultRemainingTerm).HasSnakeCaseColumnName();
            });

            modelBuilder.Entity<Liability>(e =>
            {
                e.ToTable(Keys.Entity.Liability);

                e.HasKey(x => x.Id);

                e.HasQueryFilter(x => x.UserId == Tenant.UserId
                    && !x.IsDeleted);

                e.Property(x => x.Id).HasSnakeCaseColumnName().ValueGeneratedOnAdd().IsRequired();
                e.Property(x => x.PublicKey).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.Name).HasSnakeCaseColumnName().HasMaxLength(100).IsRequired();
                e.Property(x => x.TypeName).HasSnakeCaseColumnName().HasMaxLength(100).IsRequired();
                e.Property(x => x.Value).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.MonthlyPayment).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.MonthlyPaymentEstimate).HasSnakeCaseColumnName();
                e.Property(x => x.OriginalTerm).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.RemainingTerm).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.Interest).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.AdditionalPayments).HasSnakeCaseColumnName();
                e.Property(x => x.Created).HasColumnType("timestamp").HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.UserId).HasSnakeCaseColumnName().IsRequired();
                e.Property(x => x.IsDeleted).HasSnakeCaseColumnName().IsRequired();

                e.HasOne(x => x.Type)
                    .WithMany()
                    .HasForeignKey(x => x.TypeName)
                    .IsRequired();
            });

            modelBuilder.Entity<Vehicle>(e =>
            {
                e.ToTable(Keys.Entity.Vehicle);

                e.Property(x => x.DownPayment).HasSnakeCaseColumnName().IsRequired();
            });

            modelBuilder.Entity<Loan>(e =>
            {
                e.ToTable(Keys.Entity.Loan);

                e.Property(x => x.LoanType).HasSnakeCaseColumnName().HasMaxLength(100).IsRequired();
                e.Property(x => x.ShortTerm).HasSnakeCaseColumnName();
                e.Property(x => x.LongTerm).HasSnakeCaseColumnName();
            });
        }
    }
}