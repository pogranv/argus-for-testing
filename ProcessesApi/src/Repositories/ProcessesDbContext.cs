using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProcessesApi.Models;

namespace ProcessesApi.Repositories;

public partial class ProcessesDbContext : DbContext
{
    // public ProcessesDbContext()
    // {
    // }

    public ProcessesDbContext(DbContextOptions<ProcessesDbContext> options)
        : base(options)
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Priority>("ticket_priority_type");
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    //     optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=processes_db;Username=postgres;Password=andrew7322");
    //     optionsBuilder.UseNpgsql(npgsqlOptions => npgsqlOptions.MapEnum<Priority>("ticket_priority_type"));
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Priority>("ticket_priority_type");

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.MentionedUserIds)
                .HasDefaultValueSql("'{}'::bigint[]")
                .HasColumnName("mentioned_user_ids");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Comments)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("comments_ticket_id_fkey");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("processes_pkey");

            entity.ToTable("processes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.GraphId).HasColumnName("graph_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tickets_pkey");

            entity.ToTable("tickets");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BusinessProcessId).HasColumnName("business_process_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Deadline)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deadline");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ExecutorId).HasColumnName("executor_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NotificationIds)
                .HasDefaultValueSql("'{}'::bigint[]")
                .HasColumnName("notification_ids");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.BusinessProcess).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.BusinessProcessId)
                .HasConstraintName("tickets_business_process_id_fkey");
            entity.Property(e => e.Priority)
                .HasColumnName("ticket_priority")
                .HasColumnType("ticket_priority_type"); 
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
