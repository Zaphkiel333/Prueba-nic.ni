using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Monografia.Models;

namespace test.Entities;

public partial class BookStoreContext : IdentityDbContext<ApplicationUser>
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__book__3213E83F8E3ED3F1");

            entity.ToTable("book");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasColumnType("text")
                .HasColumnName("author");
            entity.Property(e => e.Tittle)
                .HasColumnType("text")
                .HasColumnName("tittle");
            entity.Property(e => e.UnitsAvailables).HasColumnName("units_availables");
            entity.Property(e => e.YearOfRelease)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("year_of_release");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__transact__3213E83FC56A94CE");

            entity.ToTable("transaction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Book).HasColumnName("book");
            entity.Property(e => e.QuantitySold).HasColumnName("quantity_sold");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("date")
                .HasColumnName("transaction_date");

            entity.HasOne(d => d.BookNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Book)
                .HasConstraintName("FK__transactio__book__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
