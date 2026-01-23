using Microsoft.EntityFrameworkCore;
using BookingService.Core.Entities;

namespace BookingService.Infrastructure.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    public DbSet<Booking> Bookings { get; set; }
}
