using Microsoft.EntityFrameworkCore;
using Hope.Models;

namespace Hope;

public class DataContext : DbContext
{
    public DbSet<Headline>? Headlines { get; set; }

    public DataContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlite("Data Source=./data.db");
    }
}
