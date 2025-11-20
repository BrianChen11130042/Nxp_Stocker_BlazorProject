using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public class NxpMachineDbContext : DbContext
    {
        public NxpMachineDbContext(DbContextOptions<NxpMachineDbContext> options) : base(options)
        {

        }

        public virtual DbSet<MissionAsignTable> MissionAsignTables { get; set; }

        public virtual DbSet<MissionBase> MissionBases { get; set; }

        public virtual DbSet<LogTable> LogTables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MissionAsignTable>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Barcode).IsUnique();
            });

            modelBuilder.Entity<MissionBase>(entity =>
            {
                entity.UseTpcMappingStrategy();

                entity.HasKey(e => e.Id);

                entity.HasOne(x => x.MissionAsign).WithMany(x => x.Missions).HasForeignKey(x => x.AsignId);
            });

            modelBuilder.Entity<PierMissionTable>(entity =>
            {
                //entity.ToTable("PierMissionTable");
            });

            modelBuilder.Entity<RobotMissionTable>(entity =>
            {
                //entity.ToTable("RobotMissionTable");
            });

            modelBuilder.Entity<LogTable>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .UseIdentityColumn(); // 自動遞增

                //entity.ToTable("LogTable");

                entity.Property(e => e.Equipment).HasMaxLength(200);
                entity.Property(e => e.LogType).HasMaxLength(200);
                entity.Property(e => e.RecordTime).HasColumnType("datetime2");
            });

        }
    }
}
