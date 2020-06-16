using Microsoft.EntityFrameworkCore;

namespace Test2.Models
{
    public class MusicDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Musician> Musicians { get; set; }
        public DbSet<MusicLabel> MusicLabels { get; set; }
        public DbSet<MusicianTrack> MusicianTracks { get; set; }

        public MusicDbContext() { }

        public MusicDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Musician>(opt =>
            {
                opt.HasKey(p => p.IdMusician);
                opt.Property(p => p.IdMusician)
                   .ValueGeneratedOnAdd();

                opt.Property(p => p.FirstName)
                   .HasMaxLength(30)
                   .IsRequired();

                opt.Property(p => p.LastName)
                   .HasMaxLength(50)
                   .IsRequired();

                opt.Property(p => p.Nickname)
                   .HasMaxLength(20);
            });

            modelBuilder.Entity<MusicianTrack>(opt =>
            {
                opt.HasKey(p => p.IdMusicianTrack);
                opt.Property(p => p.IdMusicianTrack)
                   .ValueGeneratedOnAdd();

                opt.HasOne(p => p.Musician)
                   .WithMany(p => p.MusicianTracks)
                   .HasForeignKey(p => p.IdMusician);

                opt.HasOne(p => p.Track)
                   .WithMany(p => p.MusicianTracks)
                   .HasForeignKey(p => p.IdTrack);
            });

            modelBuilder.Entity<Track>(opt =>
            {
                opt.HasKey(p => p.IdTrack);
                opt.Property(p => p.IdTrack)
                   .ValueGeneratedOnAdd();

                opt.Property(p => p.TrackName)
                   .HasMaxLength(20)
                   .IsRequired();

                opt.HasOne(p => p.Album)
                   .WithMany(p => p.Tracks)
                   .HasForeignKey(p => p.IdMusicAlbum);
            });

            modelBuilder.Entity<Album>(opt =>
            {
                opt.HasKey(p => p.IdAlbum);
                opt.Property(p => p.IdAlbum)
                   .ValueGeneratedOnAdd();

                opt.Property(p => p.AlbumName)
                   .HasMaxLength(30)
                   .IsRequired();
            });

            modelBuilder.Entity<MusicLabel>(opt =>
            {
                opt.HasKey(p => p.IdMusicLabel);
                opt.Property(p => p.IdMusicLabel)
                   .ValueGeneratedOnAdd();

                opt.Property(p => p.Name)
                   .HasMaxLength(50)
                   .IsRequired();

                opt.HasMany(p => p.Albums)
                   .WithOne(p => p.MusicLabel)
                   .HasForeignKey(p => p.IdMusicLabel);
            });
        }
    }
}
