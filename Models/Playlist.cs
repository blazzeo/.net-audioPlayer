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
    private bool _shuffling = false;
    private ObservableCollection<TrackInfo> _tracklist;

    public bool Active { get; private set; }
    public string? Name { get => _name; set => _name = value!; }

    public ObservableCollection<TrackInfo> TrackList { get => _tracklist; }

    public PlayList()
    {
        _name = "null";
        _cover = new Bitmap(new MemoryStream(File.ReadAllBytes("Assets/default-audio.png")));
        _tracklist = new();
    }

    public PlayList(List<TrackInfo> tracklist, string imagePath)
    {
        _name = "undef";
        _cover = new Bitmap(new MemoryStream(File.ReadAllBytes(imagePath)));
        _tracklist = new(tracklist);
    }

    public PlayList(string name, string folder, string imagePath)
    {
        _name = name;
        _cover = new Bitmap(new MemoryStream(File.ReadAllBytes(imagePath)));
        _tracklist = _tracklist ?? new();
        var files = Directory.GetFiles(folder);
        
        foreach (var file in files)
        {
            if (!IsAudioFile(file)) continue;
            var track = new TrackInfo(file);
            _tracklist?.Add(track);
        }
    }

    public void addAudioFile(TrackInfo track)
    {
        _tracklist?.Add(track);
    }

    public void removeAudioFile(TrackInfo track)
    {
        _tracklist?.Remove(track);
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

    public List<TrackInfo> ToggleShuffle()
    {
        if (_shuffling)
        {
            _shuffling = !_shuffling;
            return GetShufflePlaylist();
        }
        else
        {
            _shuffling = !_shuffling;
            return new(_tracklist);
        }
    }

    private List<TrackInfo> GetShufflePlaylist()
    {
        ObservableCollection<TrackInfo> tempList = new(_tracklist);
        int n = tempList.Count;
        Random random = new Random();

        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            TrackInfo temp = tempList[k];
            tempList[k] = tempList[n];
            tempList[n] = temp;
        }

        List<TrackInfo> shuffledList = new(tempList);
        return shuffledList;
    }

    public int GetTrackID(TrackInfo track)
    {
        return _tracklist.IndexOf(track);
    }
}
