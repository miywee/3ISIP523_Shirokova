using System;
using System.Collections.Generic;
using ISIP523_Shirokova.Entities;
using Microsoft.EntityFrameworkCore;

namespace ISIP523_Shirokova.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Detail> Details { get; set; }

    public virtual DbSet<Garage> Garages { get; set; }

    public virtual DbSet<GarageDetail> GarageDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=pr7.2;Username=postgres;Password=0000");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Detail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Details_pkey");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Garage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Garage_pkey");

            entity.ToTable("Garage");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<GarageDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GarageDetails_pkey");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
            entity.Property(e => e.DetailsId).HasColumnName("DetailsID");
            entity.Property(e => e.GarageId).HasColumnName("GarageID");

            entity.HasOne(d => d.Details).WithMany(p => p.GarageDetails)
                .HasForeignKey(d => d.DetailsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageDetails_DetailsID_fkey");

            entity.HasOne(d => d.Garage).WithMany(p => p.GarageDetails)
                .HasForeignKey(d => d.GarageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GarageDetails_GarageID_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
