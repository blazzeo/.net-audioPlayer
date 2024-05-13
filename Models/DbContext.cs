using System;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayer.Models;

public class DbContext : DbContext
{
    public DbSet<PlayList> PlayLists { get; set; }
    public DbSet<TrackInfo> Tracks { get; set; }
    private string DbPath { get; }

    public DbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "local.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseMySQL($"Data Source={DbPath}");
}