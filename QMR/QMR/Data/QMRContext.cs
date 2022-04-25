using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QMR.Models;

namespace QMR.Data
{
    public partial class QMRContext : DbContext
    {
        public QMRContext()
        {
        }

        public QMRContext(DbContextOptions<QMRContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MeasureAction> measureAction { get; set; }
        public virtual DbSet<MeasureGroups> MeasureGroups { get; set; }
        public virtual DbSet<MeasureType> MeasureType { get; set; }
        public virtual DbSet<Measure> Measure { get; set; }
        public virtual DbSet<Season> Season { get; set; }
        public virtual DbSet<RollUpMeasures> RollUpMeasures { get; set; }
        public virtual DbSet<QuarterlyMeasure> QuarterlyMeasure { get; set; }
        public virtual DbSet<QuarterlyReviewDetails> QuarterlyReviewDetails { get; set; }
        public virtual DbSet<Trend> Trend { get; set; }
        public virtual DbSet<CurrentValueOption> CurrentValueOption { get; set; }
        public virtual DbSet<MeasureHistory> MeasureHistory { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrentValueOption>(entity =>
            {
                entity.HasKey(e => e.CurrentValueOptionId)
                      .HasName("measuresType$PrimaryKey");
                entity.Property(e => e.CurrentValueOptionId).HasColumnName("CurrentValueOptionId");
                entity.Property(e => e.CurrentValueOptionDescription).HasMaxLength(255);
                entity.Property(e => e.CurrentValueOptionId).IsRequired();
                entity.Property(e => e.CurrentValueOptionDescription).IsRequired();
            });

            modelBuilder.Entity<Models.MeasureAction>(entity =>
            {
                entity.HasKey(e => e.ActionId).HasName("MeasureAction$PrimaryKey");

                entity.Property(e => e.ActionId).HasColumnName("ActionID");
                entity.Property(e => e.ActionColor).HasMaxLength(50);

                entity.Property(e => e.ActionName).IsRequired();
                entity.Property(e => e.ActionStatus).IsRequired();
            });

            modelBuilder.Entity<MeasureGroups>(entity =>
            { entity.HasKey(e => e.MeasureGroupId)
                    .HasName("measuresGroups$PrimaryKey");
            
                entity.HasKey(e => e.MeasureGroupId).HasName("MeasureGroups$PrimaryKey");

                entity.Property(e => e.MeasureGroupId).HasColumnName("MeasureGroupID");

                entity.Property(e => e.MeasureGroupName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MeasureTypeId).HasColumnName("MeasureTypeID");
                
            });

            modelBuilder.Entity<MeasureType>(entity =>
            {
                entity.HasKey(e => e.MeasureTypeId)
                      .HasName("measuresType$PrimaryKey");
                entity.Property(e => e.MeasureTypeId).HasColumnName("MeasureTypeID");

                entity.Property(e => e.MeasureTypeDescription).HasMaxLength(255);
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.HasKey(e => e.SeasonId)
                        .HasName("Season$PrimaryKey");
                entity.Property(e => e.SeasonId).HasColumnName("SeasonId");
                entity.Property(e => e.SeasonName).HasMaxLength(255);
                entity.Property(e => e.SeasonId).IsRequired();
                entity.Property(e => e.SeasonName).IsRequired();
            });
            modelBuilder.Entity<RollUpMeasures>(entity =>
            {
                entity.HasKey(e => e.Id)
                        .HasName("RollUpMeasures$PrimaryKey");
                entity.Property(e => e.RollUpMeasureId).IsRequired();
                entity.Property(e => e.MeasureId).IsRequired();
            });

            modelBuilder.Entity<Measure>(entity =>
            {
                entity.HasKey(e => e.MeasureId)
                    .HasName("measures$PrimaryKey");

                entity.Property(e => e.MeasureId).HasColumnName("MeasureID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.MeasureGroupId).HasColumnName("MeasureGroupID");

                entity.Property(e => e.CurrentValueOptionId).HasColumnName("CurrentValueOptionId")
                .IsRequired();


                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<QuarterlyMeasure>(entity =>
            {
                entity.HasKey(e => e.QuarterId).HasName("QuarterlyMeasure$PrimaryKey");

                entity.Property(e => e.Quarter).HasMaxLength(50);

                entity.Property(e => e.Quarter).IsRequired();
                entity.Property(e => e.Year).IsRequired();
            });

            modelBuilder.Entity<QuarterlyReviewDetails>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("QuarterlyReviewDetails$PrimaryKey");
                entity.Property(e => e.QuarterId).IsRequired();
                entity.Property(e => e.MeasureId).IsRequired();
            });

            modelBuilder.Entity<Trend>(entity =>
            {
                entity.HasKey(e => e.TrendId).HasName("Trend$PrimaryKey");
            });

            modelBuilder.Entity<MeasureHistory>(entity =>
            {
                entity.HasNoKey();
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

       
    }
}
