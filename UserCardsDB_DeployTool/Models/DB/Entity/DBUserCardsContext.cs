using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using UserCardsDB_DeployTool.Managers;

#nullable disable

namespace UserCardsDB_DeployTool.Models.DB.Entity;

public partial class DBUserCardsContext : DbContext
{
    private string _containerDB;

    public DBUserCardsContext(string containerDB)
    {
        _containerDB = containerDB;

        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public virtual DbSet<CardHolder> CardHolders { get; set; }
    public virtual DbSet<CardInfo> CardsInfo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_containerDB);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardHolder>(entity =>
        {
            entity.HasKey(ch => ch.UID);

            entity.ToTable("card_holders");
        });

        modelBuilder.Entity<CardInfo>(entity =>
        {
            entity.HasKey(ci => ci.ID);

            entity.ToTable("cards_info");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
