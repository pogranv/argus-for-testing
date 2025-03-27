using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace StatusesApi.Repositories;

public partial class StatusDbContext : DbContext
{
    // public StatusDbContext()
    // {
    // }

    public StatusDbContext(DbContextOptions<StatusDbContext> options)
        : base(options)
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<NotificationType>("status_type"); // Укажите правильное имя типа
    }

    public virtual DbSet<Graph> Graphs { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusFlow> StatusFlows { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql(
    //         // "Host=localhost;Port=5432;Database=statuses_db;Username=postgres;Password=andrew7322",
    //         npgsqlOptions => npgsqlOptions.MapEnum<NotificationType>("status_type") // Явное указание маппинга
    //     );
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<NotificationType>("status_type");

        modelBuilder.Entity<Graph>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("graph_pkey");

            entity.ToTable("graph");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });


        modelBuilder
            .HasPostgresEnum("notification_type", new[] { "phone", "email", "sms" });


        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DutyId).HasColumnName("duty_id");
            entity.Property(e => e.IntervalMinutes).HasColumnName("interval_minutes");
            entity.Property(e => e.MentionedUserIds)
                .HasDefaultValueSql("'{}'::integer[]")
                .HasColumnName("mentioned_user_ids");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.SlaMinutes).HasColumnName("sla_minutes");
             entity.Property(e => e.NotificationType)
                .HasColumnName("notification_type")
                .HasColumnType("status_type");
                // .HasConversion(
                //     v => v.ToString(),   
                //     v => (NotificationType)Enum.Parse(typeof(NotificationType), v) 
                // );
        });

        modelBuilder.Entity<StatusFlow>(entity =>
        {
            entity.HasKey(e => new { e.GraphId, e.StatusId }).HasName("status_flow_pkey");

            entity.ToTable("status_flow");

            entity.Property(e => e.GraphId).HasColumnName("graph_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.NextStatusIds)
                .HasDefaultValueSql("'{}'::uuid[]")
                .HasColumnName("next_status_ids");
            entity.Property(e => e.OrderNum).HasColumnName("order_num");

            entity.HasOne(d => d.Graph).WithMany(p => p.StatusFlows)
                .HasForeignKey(d => d.GraphId)
                .HasConstraintName("status_flow_graph_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.StatusFlows)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("status_flow_status_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
