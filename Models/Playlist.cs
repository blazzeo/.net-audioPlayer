using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System;
using Avalonia.Media.Imaging;

namespace AudioPlayer.Models;

public partial class PlayList
{
    private string _name;

    public string? Name { get => _name; set => _name = value!; }
    public Bitmap Image { get; set; }

    public ObservableCollection<TrackInfo> TrackList { get; private set; }
}

public partial class PlayList
{
    public PlayList()
    {
        _name = "No Playlist";
        Image = new Bitmap(new MemoryStream(File.ReadAllBytes("Assets/default-audio.png")));
        TrackList = [];
    }

    public PlayList(string name, string folder, string imagePath)
    {
        _name = name;
        Image = new Bitmap(new MemoryStream(File.ReadAllBytes(imagePath)));
        TrackList = [];
        var files = Directory.GetFiles(folder);
        
        foreach (var file in files)
        {
            if (!IsAudioFile(file)) continue;
            var track = new TrackInfo(file);
            TrackList?.Add(track);
        }
    }
    
    public PlayList(string name, IEnumerable<TrackInfo>? trackList, Bitmap image)
    {
        _name = name;
        Image = image;
        if (trackList != null) TrackList = new ObservableCollection<TrackInfo>(trackList);
    }

    // public void AddAudioFile(TrackInfo track)
    // {
    //     TrackList?.Add(track);
    // }
    //
    // public void RemoveAudioFile(TrackInfo track)
    // {
    //     TrackList?.Remove(track);
    // }

    public static bool IsAudioFile(string? filePath)
    {
        HashSet<string> audioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".mp3",
            ".wav",
            ".flac",
            ".aac",
            ".wma"
        };

        var extension = Path.GetExtension(filePath);

        return audioExtensions.Contains(extension);
    }

    public List<TrackInfo> GetTrackList()
    {
        List<TrackInfo> list = [.. TrackList];
        return list;
    }

    public int GetTrackId(TrackInfo track)
    {
        return TrackList.IndexOf(track);
    }
}