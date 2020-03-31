using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PZero.dtb.Entities
{
    public partial class PZeroContext : DbContext
    {
        public PZeroContext()
        {
        }

        public PZeroContext(DbContextOptions<PZeroContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<OrderData> OrderData { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Secret.SecretString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustId)
                    .HasName("PK__Customer__049E3A899E8D2DF2");

                entity.Property(e => e.CustId).HasColumnName("CustID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocId)
                    .HasName("PK__Location__6A46DEE9E1DD74F1");

                entity.Property(e => e.LocId).HasColumnName("LocID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderData>(entity =>
            {
                entity.HasKey(e => e.DataId)
                    .HasName("PK__OrderDat__9D05305D44064362");

                entity.Property(e => e.DataId).HasColumnName("DataID");

                entity.Property(e => e.OrdId).HasColumnName("OrdID");

                entity.Property(e => e.PrdId).HasColumnName("PrdID");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Ord)
                    .WithMany(p => p.OrderData)
                    .HasForeignKey(d => d.OrdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderData__OrdID__7B5B524B");

                entity.HasOne(d => d.Prd)
                    .WithMany(p => p.OrderData)
                    .HasForeignKey(d => d.PrdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderData__PrdID__7C4F7684");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrdId)
                    .HasName("PK__orders__67A28316E6864190");

                entity.ToTable("orders");

                entity.Property(e => e.OrdId).HasColumnName("OrdID");

                entity.Property(e => e.CustId).HasColumnName("CustID");

                entity.Property(e => e.Stamp).HasColumnType("date");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__orders__CustID__787EE5A0");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.PrdId)
                    .HasName("PK__Products__7168B1043AC0ED1C");

                entity.Property(e => e.PrdId).HasColumnName("PrdID");

                entity.Property(e => e.LocId).HasColumnName("LocID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Loc)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.LocId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products__LocID__6383C8BA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
