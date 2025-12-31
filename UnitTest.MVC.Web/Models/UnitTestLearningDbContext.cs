using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.MVC.Web.Models;

public partial class UnitTestLearningDbContext : DbContext
{

    //Scaffold-DbContext "Data Source=.;Initial Catalog=UnitTestLearningDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30" Microsoft.EntityFrameworkCore.SqlServer         -OutputDir Models -Context UnitTestLearningDbContext
    public UnitTestLearningDbContext()
    {
    }

    public UnitTestLearningDbContext(DbContextOptions<UnitTestLearningDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
