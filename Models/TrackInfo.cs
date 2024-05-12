using System;
using System.IO;
using ATL;
using Avalonia.Media.Imaging;

namespace AudioPlayer.Models;

public class TrackInfo
{
    public string Path { get; private set; }
    public string Title { get; private set; }
    public string Artist { get; private set; }
    public string Album { get; private set; }
    public int Duration { get; private set; }
    public string DurationTimeSpan
    {
        get => TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss");
    }
    public Bitmap Image { get; private set; }

    public TrackInfo(Track track)
    {
        Path = track.Path;
        Title = track.Title;
        Artist = track.Artist;
        Album = track.Album;
        Duration = track.Duration;

        MemoryStream imageData;
        if (System.IO.Path.GetExtension(track.Path) == "mp3")
        {
            imageData = new MemoryStream(track.EmbeddedPictures[0].PictureData);
        }
        else
        {
            imageData = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        }

        Image = new Bitmap(imageData);
    }

    public TrackInfo(string path)
    {
        Track track = new Track(path);

        Path = track.Path;
        Title = track.Title;
        Artist = track.Artist;
        Album = track.Album;
        Duration = track.Duration;

        MemoryStream imageData;
        if (System.IO.Path.GetExtension(track.Path) == ".mp3")
        {
            imageData = new MemoryStream(track.EmbeddedPictures[0].PictureData);
        }
        else
        {
            imageData = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        }

        Image = new Bitmap(imageData);
    }

    public TrackInfo()
    {
        Track track = new Track();

        Path = Title = Album = Artist = null;
        Duration = 0;
        MemoryStream imageData;
        imageData = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        Image = new Bitmap(imageData);
    }
}