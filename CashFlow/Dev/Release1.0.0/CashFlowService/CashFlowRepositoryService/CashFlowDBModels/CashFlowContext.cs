using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CashFlowRepositoryService.CashFlowDBModels
{
    public partial class CashFlowContext : DbContext
    {
        public CashFlowContext()
        {
        }

        public CashFlowContext(DbContextOptions<CashFlowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actuals> Actuals { get; set; }
        public virtual DbSet<Associations> Associations { get; set; }
        public virtual DbSet<CfaGrid> CfaGrid { get; set; }
        public virtual DbSet<Errors> Errors { get; set; }
        public virtual DbSet<Exposures> Exposures { get; set; }
        public virtual DbSet<ProdActuals> ProdActuals { get; set; }
        public virtual DbSet<ProdExposures> ProdExposures { get; set; }
        public virtual DbSet<ProdWorkMatters> ProdWorkMatters { get; set; }
        public virtual DbSet<Substitutions> Substitutions { get; set; }
        public virtual DbSet<Superusers> Superusers { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WorkMatters> WorkMatters { get; set; }

        // Unable to generate entity type for table 'data.ProdNotifications'. Please see the warning messages.
        // Unable to generate entity type for table 'data.ValuationPeriod'. Please see the warning messages.
        // Unable to generate entity type for table 'data.Permissions'. Please see the warning messages.
        // Unable to generate entity type for table 'data.CashFlowEntry'. Please see the warning messages.
        // Unable to generate entity type for table 'data.Notifications'. Please see the warning messages.
        // Unable to generate entity type for table 'data.ProdCashFlowEntry'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source = sqldev2012r2; Initial Catalog = CashFlow; User Id = cashflow; Password=cashflow");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actuals>(entity =>
            {
                entity.HasKey(e => new { e.ExpId, e.Year, e.Quarter });

                entity.ToTable("Actuals", "data");

                entity.Property(e => e.ExpId)
                    .HasColumnName("ExpID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ExposureId).HasColumnName("Exposure_ID");

                entity.Property(e => e.PaidDjExp).HasColumnName("Paid_DJ_Exp");

                entity.Property(e => e.PaidNonDjExp).HasColumnName("Paid_NonDJ_Exp");

                entity.Property(e => e.PaidNonDjExpOutsideLimits).HasColumnName("Paid_NonDJ_Exp_Outside_Limits");

                entity.Property(e => e.PaidNonDjExpWithinLimits).HasColumnName("Paid_NonDJ_Exp_Within_Limits");

                entity.Property(e => e.TotalPaidLosses).HasColumnName("Total_Paid_Losses");
            });

            modelBuilder.Entity<Associations>(entity =>
            {
                entity.HasKey(e => new { e.AssociationId, e.WorkMatter });

                entity.ToTable("Associations", "data");

                entity.HasIndex(e => e.AssociationId)
                    .HasName("IX_Associations");

                entity.HasIndex(e => e.WorkMatter)
                    .HasName("IX_Associations_1");

                entity.Property(e => e.AssociationId).HasColumnName("AssociationID");

                entity.Property(e => e.WorkMatter)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CfaGrid>(entity =>
            {
                entity.ToTable("CfaGrid", "data");

                entity.Property(e => e.CfaGridId).HasColumnName("CfaGridID");

                entity.Property(e => e.CfaGridKey)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CfaGridReadWrite)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CfaGridType)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Errors>(entity =>
            {
                entity.HasKey(e => new { e.ActiveDirectoryId, e.OccurenceTime });

                entity.ToTable("Errors", "data");

                entity.Property(e => e.ActiveDirectoryId)
                    .HasColumnName("ActiveDirectoryID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OccurenceTime).HasColumnType("datetime");

                entity.Property(e => e.AdjustedName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Feature)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StackTrace)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Exposures>(entity =>
            {
                entity.HasKey(e => e.ExpId);

                entity.ToTable("Exposures", "data");

                entity.HasIndex(e => e.WorkMatter)
                    .HasName("IX_Exposures");

                entity.Property(e => e.ExpId)
                    .HasColumnName("ExpID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AttachPoint).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Coverage)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.ExpClosedDate).HasColumnType("datetime");

                entity.Property(e => e.PolicyNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Portfolio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WithinLimits)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.WithinLimitsSource)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.WorkMatter)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProdActuals>(entity =>
            {
                entity.HasKey(e => new { e.ExpId, e.Year, e.Quarter });

                entity.ToTable("ProdActuals", "data");

                entity.Property(e => e.ExpId)
                    .HasColumnName("ExpID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ExposureId).HasColumnName("Exposure_ID");

                entity.Property(e => e.PaidDjExp).HasColumnName("Paid_DJ_Exp");

                entity.Property(e => e.PaidNonDjExp).HasColumnName("Paid_NonDJ_Exp");

                entity.Property(e => e.PaidNonDjExpOutsideLimits).HasColumnName("Paid_NonDJ_Exp_Outside_Limits");

                entity.Property(e => e.PaidNonDjExpWithinLimits).HasColumnName("Paid_NonDJ_Exp_Within_Limits");

                entity.Property(e => e.TotalPaidLosses).HasColumnName("Total_Paid_Losses");
            });

            modelBuilder.Entity<ProdExposures>(entity =>
            {
                entity.HasKey(e => e.ExpId);

                entity.ToTable("ProdExposures", "data");

                entity.Property(e => e.ExpId)
                    .HasColumnName("ExpID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AttachPoint).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Coverage)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.ExpClosedDate).HasColumnType("datetime");

                entity.Property(e => e.PolicyNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Portfolio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WithinLimits)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.WithinLimitsSource)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.WorkMatter)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProdWorkMatters>(entity =>
            {
                entity.HasKey(e => e.WorkMatter);

                entity.ToTable("ProdWorkMatters", "data");

                entity.Property(e => e.WorkMatter)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignedAdjuster)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedManager)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EndUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuredName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Portfolio)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SpecialTrackingGroup)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.StartUser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Wmclosed).HasColumnName("WMClosed");

                entity.Property(e => e.WmclosedDate)
                    .HasColumnName("WMClosedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.WorkMatterDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Substitutions>(entity =>
            {
                entity.HasKey(e => e.ActiveDirectoryId);

                entity.ToTable("Substitutions", "data");

                entity.Property(e => e.ActiveDirectoryId)
                    .HasColumnName("ActiveDirectoryID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.SubActiveDirectoryId)
                    .IsRequired()
                    .HasColumnName("SubActiveDirectoryID")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Superusers>(entity =>
            {
                entity.HasKey(e => new { e.Associate, e.Department });

                entity.ToTable("Superusers", "data");

                entity.Property(e => e.Associate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Department)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Access)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.ActiveDirectoryId);

                entity.ToTable("Users", "data");

                entity.Property(e => e.ActiveDirectoryId)
                    .HasColumnName("ActiveDirectoryID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdjustedName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EndUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SamAccountName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StartUser)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SupervisorId)
                    .IsRequired()
                    .HasColumnName("SupervisorID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TeamManagerId)
                    .IsRequired()
                    .HasColumnName("TeamManagerID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UnitManagerId)
                    .IsRequired()
                    .HasColumnName("UnitManagerID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<WorkMatters>(entity =>
            {
                entity.HasKey(e => e.WorkMatter);

                entity.ToTable("WorkMatters", "data");

                entity.HasIndex(e => e.AssignedAdjuster)
                    .HasName("IX_WorkMatters");

                entity.Property(e => e.WorkMatter)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignedAdjuster)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedManager)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HasAssociations).HasColumnName("HasAssociations");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EndUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuredName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Portfolio)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SpecialTrackingGroup)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.StartUser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Wmclosed).HasColumnName("WMClosed");

                entity.Property(e => e.WmclosedDate)
                    .HasColumnName("WMClosedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.WorkMatterDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
