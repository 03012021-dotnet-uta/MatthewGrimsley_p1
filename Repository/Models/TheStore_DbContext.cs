using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Repository.Models
{
    public partial class TheStore_DbContext : DbContext
    {
        public TheStore_DbContext()
        {
        }

        public TheStore_DbContext(DbContextOptions<TheStore_DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<PartsInOrder> PartsInOrders { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configured in WebAPI/Startup.cs for DI
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountNumber)
                    .HasName("PK__accounts__AF91A6AC0A0E1BD1");

                entity.ToTable("accounts");

                entity.Property(e => e.AccountNumber).HasColumnName("account_number");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.DefaultStore).HasColumnName("default_store");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("hash")
                    .IsFixedLength(true);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Permissions).HasColumnName("permissions");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("numeric(14, 0)")
                    .HasColumnName("phone_number");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("salt")
                    .IsFixedLength(true);

                entity.Property(e => e.StateName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("state_name");

                entity.Property(e => e.StreetAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("street_address");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.ZipCode)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("zip_code");

                entity.HasOne(d => d.DefaultStoreNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.DefaultStore)
                    .HasConstraintName("FK__accounts__defaul__0E6E26BF");

                entity.HasOne(d => d.StateNameNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.StateName)
                    .HasConstraintName("FK__accounts__state___0D7A0286");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.PartNumber })
                    .HasName("store_part_pk");

                entity.ToTable("inventory");

                entity.Property(e => e.StoreNumber).HasColumnName("store_number");

                entity.Property(e => e.PartNumber).HasColumnName("part_number");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.PartNumberNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.PartNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__inventory__part___123EB7A3");

                entity.HasOne(d => d.StoreNumberNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.StoreNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__inventory__store__114A936A");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderNumber)
                    .HasName("PK__orders__730E34DE5A844F21");

                entity.ToTable("orders");

                entity.Property(e => e.OrderNumber).HasColumnName("order_number");

                entity.Property(e => e.AccountNumber).HasColumnName("account_number");

                entity.Property(e => e.DateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("date_time");

                entity.Property(e => e.StoreNumber).HasColumnName("store_number");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("smallmoney")
                    .HasColumnName("subtotal");

                entity.Property(e => e.Tax)
                    .HasColumnType("smallmoney")
                    .HasColumnName("tax");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("smallmoney")
                    .HasColumnName("total_price");

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AccountNumber)
                    .HasConstraintName("FK__orders__account___151B244E");

                entity.HasOne(d => d.StoreNumberNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreNumber)
                    .HasConstraintName("FK__orders__store_nu__160F4887");
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.HasKey(e => e.PartNumber)
                    .HasName("PK__parts__813D5BC9BA9A53B8");

                entity.ToTable("parts");

                entity.Property(e => e.PartNumber).HasColumnName("part_number");

                entity.Property(e => e.ImageCredit)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("image_credit");

                entity.Property(e => e.ImageLink)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("image_link");

                entity.Property(e => e.PartDescription)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("part_description");

                entity.Property(e => e.PartName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("part_name");

                entity.Property(e => e.SalePercent)
                    .HasColumnType("numeric(5, 3)")
                    .HasColumnName("sale_percent")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.UnitOfMeasure)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("unit_of_measure");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("smallmoney")
                    .HasColumnName("unit_price");
            });

            modelBuilder.Entity<PartsInOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderNumber, e.PartNumber })
                    .HasName("order_part_pk");

                entity.ToTable("parts_in_orders");

                entity.Property(e => e.OrderNumber).HasColumnName("order_number");

                entity.Property(e => e.PartNumber).HasColumnName("part_number");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UnitOfMeasure)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("unit_of_measure");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("smallmoney")
                    .HasColumnName("unit_price");

                entity.HasOne(d => d.OrderNumberNavigation)
                    .WithMany(p => p.PartsInOrders)
                    .HasForeignKey(d => d.OrderNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parts_in___order__18EBB532");

                entity.HasOne(d => d.PartNumberNavigation)
                    .WithMany(p => p.PartsInOrders)
                    .HasForeignKey(d => d.PartNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__parts_in___part___19DFD96B");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.StateName)
                    .HasName("PK__states__8D2CE19B0757293F");

                entity.ToTable("states");

                entity.Property(e => e.StateName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("state_name");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("abbreviation")
                    .IsFixedLength(true);

                entity.Property(e => e.TaxPercent)
                    .HasColumnType("numeric(5, 3)")
                    .HasColumnName("tax_percent");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.StoreNumber)
                    .HasName("PK__stores__0BBE57CCD22ABEEB");

                entity.ToTable("stores");

                entity.Property(e => e.StoreNumber).HasColumnName("store_number");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.StateName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("state_name");

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("store_name");

                entity.Property(e => e.StreetAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("street_address");

                entity.Property(e => e.ZipCode)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("zip_code");

                entity.HasOne(d => d.StateNameNavigation)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.StateName)
                    .HasConstraintName("FK__stores__state_na__0A9D95DB");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
