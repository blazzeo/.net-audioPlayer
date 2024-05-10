using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using AudioPlayer.Templates;
using System.IO;
using System;
using Avalonia.Media.Imaging;

namespace AudioPlayer.Models;

public class PlayList : INotifyPropertyChanged
{
    private string _name;
    private Bitmap _cover;

    public string? Name { get => _name; set => _name = value!; }
    public Bitmap Image { get => _cover; private set => _cover = value; }

    public ObservableCollection<TrackInfo> TrackList { get; private set; }

    public PlayList()
    {
        _name = "null";
        _cover = new Bitmap(new MemoryStream(File.ReadAllBytes("Assets/default-audio.png")));
        TrackList = new();
    }

    public PlayList(string name, string folder, string imagePath)
    {
        _name = name;
        _cover = new Bitmap(new MemoryStream(File.ReadAllBytes(imagePath)));
        TrackList = TrackList ?? new();
        var files = Directory.GetFiles(folder);
        
        foreach (var file in files)
        {
            if (!IsAudioFile(file)) continue;
            var track = new TrackInfo(file);
            TrackList?.Add(track);
        }
    }
    
    public PlayList(string name, string folder, Bitmap image)
    {
        _name = name;
        _cover = image;
        TrackList = TrackList ?? new();
        var files = Directory.GetFiles(folder);
        
        foreach (var file in files)
        {
            if (!IsAudioFile(file)) continue;
            var track = new TrackInfo(file);
            TrackList?.Add(track);
        }
    }

    public void AddAudioFile(TrackInfo track)
    {
        TrackList?.Add(track);
    }

    public void RemoveAudioFile(TrackInfo track)
    {
        TrackList?.Remove(track);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    static bool IsAudioFile(string filePath)
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

        return extension != null && audioExtensions.Contains(extension);
    }

    public List<TrackInfo> GetTracklist()
    {
        List<TrackInfo> list = [.. TrackList];
        return list;
    }

    public int GetTrackID(TrackInfo track)
    {
        return TrackList.IndexOf(track);
    }
}
