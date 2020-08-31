using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ApprovalSystem.Models
{
    public partial class ApprovalSystemContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApprovalSystemContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApprovalSystemContext(DbContextOptions<ApprovalSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Approval> Approval { get; set; }
        public virtual DbSet<ApprovalDetail> ApprovalDetail { get; set; }
        public virtual DbSet<ApprovalHierarchy> ApprovalHierarchy { get; set; }
        public virtual DbSet<ApprovalHierarchyDetail> ApprovalHierarchyDetail { get; set; }
        public virtual DbSet<ApprovalStatus> ApprovalStatus { get; set; }
        public virtual DbSet<ApprovalType> ApprovalType { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Approval>(entity =>
            {
                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.UpdateOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ApprovalStatus)
                    .WithMany(p => p.Approval)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .HasConstraintName("FK_Approval_ApprovalStatus");

                entity.HasOne(d => d.ApprovalType)
                    .WithMany(p => p.Approval)
                    .HasForeignKey(d => d.ApprovalTypeId)
                    .HasConstraintName("FK_Approval_ApprovalType");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.ApprovalAssignedToNavigation)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("FK_Approval_AspNetUsers2");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ApprovalCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Approval_AspNetUsers");

                entity.HasOne(d => d.UpdateByNavigation)
                    .WithMany(p => p.ApprovalUpdateByNavigation)
                    .HasForeignKey(d => d.UpdateBy)
                    .HasConstraintName("FK_Approval_AspNetUsers1");
            });

            modelBuilder.Entity<ApprovalDetail>(entity =>
            {
                entity.Property(e => e.ApprovalDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Approval)
                    .WithMany(p => p.ApprovalDetail)
                    .HasForeignKey(d => d.ApprovalId)
                    .HasConstraintName("FK_ApprovalDetail_Approval");

                entity.HasOne(d => d.ApprovalStatus)
                    .WithMany(p => p.ApprovalDetail)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .HasConstraintName("FK_ApprovalDetail_ApprovalStatus");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ApprovalDetail)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ApprovalDetail_AspNetUsers");
            });

            modelBuilder.Entity<ApprovalHierarchy>(entity =>
            {
                entity.Property(e => e.ApprovalTypeId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.ApprovalType)
                    .WithMany(p => p.ApprovalHierarchy)
                    .HasForeignKey(d => d.ApprovalTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalHierarchy_ApprovalType");
            });

            modelBuilder.Entity<ApprovalHierarchyDetail>(entity =>
            {
                entity.HasOne(d => d.ApprovalHierarchy)
                    .WithMany(p => p.ApprovalHierarchyDetail)
                    .HasForeignKey(d => d.ApprovalHierarchyId)
                    .HasConstraintName("FK_ApprovalHierarchyDetail_ApprovalHierarchy");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ApprovalHierarchyDetail)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ApprovalHierarchyDetail_AspNetUsers");
            });

            modelBuilder.Entity<ApprovalStatus>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ApprovalType>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });
        }
    }
}
