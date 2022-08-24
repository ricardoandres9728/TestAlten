using Microsoft.EntityFrameworkCore;
using TestDAL.Models;

namespace TestDAL
{
    public class TestBDContext : DbContext
    {
        public DbSet<Reservation>? Reservations { get; set; }
        public DbSet<Client>? Clients { get; set; }
        
        public TestBDContext(DbContextOptions<TestBDContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Table creacion in database
            modelBuilder.Entity<Client>(client =>
            {
                client.ToTable("Client");
                client.HasKey(x => x.Id);

                client.HasMany(x => x.Reservations).WithOne(x => x.Client).HasForeignKey(x => x.ClientId);

                client.Property(x => x.Name).IsRequired().HasMaxLength(200);
                client.Property(x => x.Identification).IsRequired().HasMaxLength(20);

                client.Ignore(x => x.Reservations);

            });

            //Table creacion in database
            modelBuilder.Entity<Reservation>(reservation =>
            {
                reservation.ToTable("Reservation");
                reservation.HasKey(x => x.Id);


                reservation.Property(x => x.ReservationDate).IsRequired().HasDefaultValue(DateTime.Today)
                .HasComment("Date indicating when the reservation was created");
                reservation.Property(x => x.StartReservation).IsRequired()
                .HasComment("Date indicating when the reservation starts");
                reservation.Property(x => x.EndReservation).IsRequired()
                .HasComment("Date indicating when the reservation ends");
                reservation.Property(x => x.ClientId).IsRequired(false)
                .HasComment("ForeignKey to the Client table");
                reservation.Property(x => x.Code).IsRequired().HasMaxLength(10)
                .HasComment("Reservation code, informative purpose");
                reservation.Property(x => x.State).IsRequired().HasDefaultValue(ReservationState.Active)
                .HasComment("Current state of the reservation 0:Active, 1:Innactive, 2: Canceled");

                //Ignores relationship, can be populated in the future
                reservation.Ignore(x => x.Client);
            });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Defines name of the databased used in Memory, for testing purposes
            optionsBuilder
                .UseInMemoryDatabase("Test", b => b.EnableNullChecks(false));
        }

    }
}
