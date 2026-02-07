using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlueCraeftBowl.Models;

namespace BlueCraeftBowl.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ParlaySession> ParlaySessions { get; set; }
    public DbSet<ParlayItem> ParlayItems { get; set; }
    public DbSet<UserSelection> UserSelections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserSelection>()
            .HasOne(us => us.User)
            .WithMany(u => u.Selections)
            .HasForeignKey(us => us.UserId);

        builder.Entity<UserSelection>()
            .HasOne(us => us.ParlayItem)
            .WithMany(pi => pi.Selections)
            .HasForeignKey(us => us.ParlayItemId);

        builder.Entity<ParlayItem>()
            .HasOne(pi => pi.Session)
            .WithMany(ps => ps.Items)
            .HasForeignKey(pi => pi.SessionId);
    }
}
