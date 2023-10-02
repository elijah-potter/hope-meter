using Microsoft.EntityFrameworkCore;
using Hope.Models;

namespace Hope;

public class DataContext : DbContext
{
    public DbSet<Headline>? Headlines { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}
