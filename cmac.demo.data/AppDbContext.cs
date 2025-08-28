using cmac.demo.datamodels;

using Microsoft.EntityFrameworkCore;

namespace cmac.demo.data;

public class AppDbContext : DbContext
{
    public DbSet<Donation> Donations { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Donation>().HasKey(p => p.Id);

        modelBuilder.Entity<PaymentMethod>().HasKey(p => p.Id);

        modelBuilder.Entity<Donation>()
                                .HasOne(d => d.PaymentMethod)
                                .WithMany() 
                                .HasForeignKey("PaymentMethodId") 
                                .IsRequired();
    }
}