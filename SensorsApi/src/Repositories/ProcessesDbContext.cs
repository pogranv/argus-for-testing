using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SensorsApi.Models;

namespace SensorsApi.src.Repositories;

public partial class SensorsDbContext : DbContext
{
    // public SensorsDbContext()
    // {
    // }

    public SensorsDbContext(DbContextOptions<SensorsDbContext> options)
        : base(options)
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Priority>("ticket_priority_type");
    }

    public virtual DbSet<Sensor> Sensors { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    //     // optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=sensors_db;Username=postgres;Password=andrew7322");
    //     optionsBuilder.UseNpgsql(
    //         "Host=localhost;Port=5432;Database=sensors_db;Username=postgres;Password=andrew7322",
    //         npgsqlOptions => npgsqlOptions.MapEnum<Priority>("ticket_priority_type") // Явное указание маппинга
    //     );
    //     // NpgsqlConnection.GlobalTypeMapper.MapEnum<Priority>("ticket_priority_type");
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Priority>("ticket_priority_type");
        // modelBuilder
        //     .HasPostgresEnum("ticket_priority_type", new[] { "low", "medium", "high" });

        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sensors_pkey");

            entity.ToTable("sensors");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BusinessProcessId).HasColumnName("business_process_id");
            entity.Property(e => e.ResolveDaysCount)
                .HasColumnName("resolve_days_count");
            entity.Property(e => e.TicketDescription).HasColumnName("ticket_description");
            entity.Property(e => e.TicketName)
                .HasMaxLength(255)
                .HasColumnName("ticket_name");
            entity.Property(e => e.Priority)
                .HasColumnName("ticket_priority")
                .HasColumnType("ticket_priority_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
