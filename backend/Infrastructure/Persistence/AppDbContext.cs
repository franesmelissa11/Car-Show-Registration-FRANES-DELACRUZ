
namespace Infrastructure.Persistence;

using Application.Abstractions.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Registration> Registrations => Set<Registration>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.ParticipantName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.ContactNumber)
                .IsRequired()
                .HasMaxLength(11);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(x => x.CarMakeModel)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.PlateNumber)
                .IsRequired()
                .HasMaxLength(15);

            entity.Property(x => x.RegistrationFee)
                .HasPrecision(10, 2);

            entity.Property(x => x.AmountPaid)
                .HasPrecision(10, 2);

            entity.HasIndex(x => x.RegistrationNumber)
                .IsUnique();

            entity.HasIndex(x => x.PlateNumber)
                .IsUnique();
        });
    }
}
