using UserCardsAPI.Managers;
using Microsoft.EntityFrameworkCore;

namespace UserCardsAPI.Models.DB.Entity
{
    public partial class DBUserCardsContext : DbContext
    {
        private String _dbConnectionString;
        public DBUserCardsContext()
        {
            var config = Config.GetInstance();
            _dbConnectionString = config.ConnectionString;
        }

        public DBUserCardsContext(DbContextOptions<DBUserCardsContext> options) : base(options)
        {
            var config = Config.GetInstance();
            _dbConnectionString = config.ConnectionString;
        }

        public virtual DbSet<CardHolder> CardHolders { get; set; } = null!;
        public virtual DbSet<CardsInfo> CardsInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_dbConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardHolder>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("card_holders");

                entity.Property(e => e.Uid).HasColumnName("UID");
            });

            modelBuilder.Entity<CardsInfo>(entity =>
            {
                entity.ToTable("cards_info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cvc).HasColumnName("CVC");

                entity.Property(e => e.Fiolat).HasColumnName("FIOLat");

                entity.Property(e => e.Uid).HasColumnName("UID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
