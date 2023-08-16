using System;
using System.Collections.Generic;
using AccountAuthMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountAuthMicroservice.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Debt> Debts { get; set; }

    public virtual DbSet<DebtDetail> DebtDetails { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DeliveryDetail> DeliveryDetails { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseDetail> ExpenseDetails { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<LoginHistory> LoginHistories { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<PurchaseType> PurchaseTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "Account_pk").IsUnique();

            entity.HasIndex(e => e.NoHp, "Account_pk2").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.MemberId).HasMaxLength(50);
            entity.Property(e => e.NoHp).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasColumnType("bit");

            entity.HasOne(d => d.Member).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Account_Member_Id_fk");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Account");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NoHp).HasMaxLength(50);
            entity.Property(e => e.StoreId).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).HasColumnType("bit");

            entity.HasOne(d => d.Store).WithMany(p => p.Customers)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Store");
        });

        modelBuilder.Entity<Debt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Debt_pk");

            entity.ToTable("Debt");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime2");
            entity.Property(e => e.StoreId).HasMaxLength(50);
            entity.Property(e => e.IsPaid).HasColumnType("bit");

            entity.HasOne(d => d.Customer).WithMany(p => p.Debts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Debt_Customer_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Debts)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Debt_Store_Id_fk");
        });

        modelBuilder.Entity<DebtDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DebtDetail_pk");

            entity.ToTable("DebtDetail");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime2");
            entity.Property(e => e.DebtId).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasMaxLength(50);

            entity.HasOne(d => d.Debt).WithMany(p => p.DebtDetails)
                .HasForeignKey(d => d.DebtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DebtDetail_Debt_Id_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.DebtDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DebtDetail_Product_Id_fk");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Delivery_pk");

            entity.ToTable("Delivery");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime2");
            entity.Property(e => e.StoreId).HasMaxLength(50);
            entity.Property(e => e.IsDelivered).HasColumnType("bit");

            entity.HasOne(d => d.Customer).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Delivery_Customer_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Delivery_Store_Id_fk");
        });

        modelBuilder.Entity<DeliveryDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DeliveryDetail_pk");

            entity.ToTable("DeliveryDetail");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.DeliveryId).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime2");
            
            entity.HasOne(d => d.Delivery).WithMany(p => p.DeliveryDetails)
                .HasForeignKey(d => d.DeliveryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DeliveryDetail_Delivery_Id_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.DeliveryDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DeliveryDetail_Product_Id_fk");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Expense_pk");

            entity.ToTable("Expense");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime2");
            entity.Property(e => e.StoreId).HasMaxLength(50);

            entity.HasOne(d => d.Store).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Expense_Store_Id_fk");
        });

        modelBuilder.Entity<ExpenseDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ExpenseDetail_pk");

            entity.ToTable("ExpenseDetail");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime2");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ExpenseId).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Expense).WithMany(p => p.ExpenseDetails)
                .HasForeignKey(d => d.ExpenseId)
                .HasConstraintName("ExpenseDetail_Expense_Id_fk");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Image_pk");

            entity.ToTable("Image");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Image___fk");
        });

        modelBuilder.Entity<LoginHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LoginHistory_pk");

            entity.ToTable("LoginHistory");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.AccountId).HasMaxLength(50);
            entity.Property(e => e.LastLogin).HasColumnType("datetime2");

            entity.HasOne(d => d.Account).WithMany(p => p.LoginHistories)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LoginHistory_Account_Id_fk");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Member_pk");

            entity.ToTable("Member");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.StoreId).HasMaxLength(50);

            entity.HasOne(d => d.Store).WithMany(p => p.Members)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Member_Store_Id_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Product_pk");

            entity.ToTable("Product");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.CreatedAccountId).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime2");
            entity.Property(e => e.EditedAccountId).HasMaxLength(50);
            entity.Property(e => e.EditedAt).HasColumnType("datetime2");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StoreId).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).HasColumnType("bit");

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.ProductCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Account_Id_fk");

            entity.HasOne(d => d.EditedAccount).WithMany(p => p.ProductEditedAccounts)
                .HasForeignKey(d => d.EditedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Account_Id_fk2");

            entity.HasOne(d => d.Store).WithMany(p => p.Products)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Store_Id_fk");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Purchase_pk");

            entity.ToTable("Purchase");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("datetime2");
            entity.Property(e => e.Money).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseTypeId).HasMaxLength(50);
            entity.Property(e => e.StoreId).HasMaxLength(50);

            entity.HasOne(d => d.PurchaseType).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.PurchaseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_PurchaseType_Id_fk");

            entity.HasOne(d => d.Store).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Purchase_Store_Id_fk");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PurchaseDetail_pk");

            entity.ToTable("PurchaseDetail");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasMaxLength(50);
            entity.Property(e => e.PurchaseId).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetail_Product_Id_fk");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PurchaseDetail_Purchase_Id_fk");
        });

        modelBuilder.Entity<PurchaseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PurchaseType_pk");

            entity.ToTable("PurchaseType");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Store");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });
    }
}
