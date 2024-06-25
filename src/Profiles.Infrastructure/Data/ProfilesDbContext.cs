using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Data;

public class ProfilesDbContext : DbContext
{
    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<BaseUser> Users { get; set; }

    public DbSet<Doctor> Doctors { get; set; }

    public DbSet<Receptionist> Receptionists { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Patient> Patients { get; set; }
}
