namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Team
            modelBuilder.Entity<Team>()
                .HasOne(e => e.PrimaryKitColor)
                .WithMany(e => e.PrimaryKitTeams)
                .HasForeignKey(e => e.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(e => e.SecondaryKitColor)
                .WithMany(e => e.SecondaryKitTeams)
                .HasForeignKey(e => e.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(e => e.Town)
                .WithMany(e => e.Teams)
                .HasForeignKey(e => e.TownId);

            //Town
            modelBuilder.Entity<Town>()
                .HasOne(e => e.Country)
                .WithMany(e => e.Towns)
                .HasForeignKey(e => e.CountryId);

            //Player
            modelBuilder.Entity<Player>()
                .HasOne(e => e.Team)
                .WithMany(e => e.Players)
                .HasForeignKey(e => e.TeamId);

            modelBuilder.Entity<Player>()
                .HasOne(e => e.Position)
                .WithMany(e => e.Players)
                .HasForeignKey(e => e.PositionId);

            //Game
            modelBuilder.Entity<Game>()
                .HasOne(e => e.HomeTeam)
                .WithMany(e => e.HomeGames)
                .HasForeignKey(e => e.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(e => e.AwayTeam)
                .WithMany(e => e.AwayGames)
                .HasForeignKey(e => e.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            //PlayerStatistic
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(e => new {e.PlayerId, e.GameId});

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(e => e.Player)
                .WithMany(e => e.PlayerStatistics)
                .HasForeignKey(e => e.PlayerId);

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(e => e.Game)
                .WithMany(e => e.PlayerStatistics)
                .HasForeignKey(e => e.GameId);

            //Bet
            modelBuilder.Entity<Bet>()
                .Property(e => e.Prediction)
                .IsRequired(true);

            modelBuilder.Entity<Bet>()
                .HasOne(e => e.User)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Bet>()
                .HasOne(e => e.Game)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.GameId);
        }
    }
}
