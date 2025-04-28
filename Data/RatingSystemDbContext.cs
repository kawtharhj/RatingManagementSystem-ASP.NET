using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Data;

public class RatingSystemDbContext : IdentityDbContext<UserData>
{

    public RatingSystemDbContext(DbContextOptions<RatingSystemDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationData>()
               .HasMany(a => a.ThesisDetails)
               .WithOne(t => t.ApplicationData)
               .HasForeignKey(t => t.ApplicationID)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Thesis>()
            .HasOne(r => r.Results)
            .WithOne(t => t.Thesis)
            .HasForeignKey<FinalResult>(rt => rt.TID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CommitteeMember>()
            .HasOne(a => a.Members)
            .WithOne(u => u.CommitteeMember)
            .HasForeignKey<UserData>(k => k.CommitteeID)
            .OnDelete(DeleteBehavior.Cascade); ;


        string[] roles = { "Admin", "Employee", "Doctor" };
        foreach (string role in roles)
        {
            builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = role,
                NormalizedName = role.ToUpper()

            }
       );
        }
    }
    public DbSet<ApplicationData> Applications { get; set; }
    public DbSet<Thesis> AppThesis { get; set; }
    public DbSet<CommitteeMember> CommitteeMembers { get; set; }
    public DbSet<FinalResult> Results { get; set; }

}
