using Microsoft.EntityFrameworkCore;

namespace AudioPlayer.Models;

public class DbConnector : DbContext
{
    public DbSet<PlayList> PlayLists => Set<PlayList>();

    public DbConnector()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("Data Source=temp.db");
    }
}