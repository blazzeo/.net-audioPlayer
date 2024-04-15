using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using AudioPlayer.Templates;
using System.IO;
using System;
using ATL;

namespace AudioPlayer.Models;

public class PlayList : INotifyPropertyChanged
{
    private string _name;
    private Control _cover;
    private bool shuffling = false;
    private ObservableCollection<Track> _tracklist;

    public bool Active { get; private set; }
    public string? Name { get => _name; set => _name = value!; }

    public ObservableCollection<Track> TrackList { get => _tracklist; }

    public PlayList()
    {
        _name = "null";
        _cover = GetImage();
        _tracklist = new();
    }

    public PlayList(List<Track> tracklist)
    {
        _name = "undef";
        _tracklist = new();
        foreach (var track in tracklist)
        {
            _tracklist.Add(track);
        }
        _cover = GetImage(_tracklist[0]);
    }

    public PlayList(string name, string folderpath)
    {
        _name = name;
        _cover = GetImage();
        _tracklist = _tracklist ?? new();
        string[] filepathes = Directory.GetFiles(folderpath);

        foreach (var file in filepathes)
        {
            if (IsAudioFile(file))
            {
                var track = new Track(file);
                _tracklist?.Add(track);
            }
        }
    }

    public void New(string name, string folderpath)
    {
        Name = name;
        string[] filepathes = Directory.GetFiles(folderpath);

        _tracklist = new();

        foreach (var file in filepathes)
        {
            if (IsAudioFile(file))
            {
                var track = new Track(file);
                _tracklist?.Add(track);
            }
        }
    }

    public void addAudioFile(Track track)
    {
        _tracklist?.Add(track);
    }

    public void removeAudioFile(Track track)
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

        string extension = Path.GetExtension(filePath);

        if (extension != null && audioExtensions.Contains(extension))
        {
            return true;
        }

        return false;
    }

    public List<Track> ToggleShuffle()
    {
        if (shuffling)
        {
            shuffling = !shuffling;
            return GetShufflePlaylist();
        }
        else
        {
            shuffling = !shuffling;
            return new(_tracklist);
        }
    }

    private List<Track> GetShufflePlaylist()
    {
        ObservableCollection<Track> tempList = new(_tracklist);
        int n = tempList.Count;
        Random random = new Random();

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Track temp = tempList[k];
            tempList[k] = tempList[n];
            tempList[n] = temp;
        }

        List<Track> shuffledList = new(tempList);
        return shuffledList;
    }

    public int GetTrackID(Track track)
    {
        return _tracklist.IndexOf(track);
    }

    private Control GetImage(Track track = null!)
    {
        MemoryStream memory;
        Avalonia.Media.Imaging.Bitmap AvIrBitmap;
        var IT = new AlbumImageTemplate();
        if (track != null)
        {
            memory = new MemoryStream(trackTag(track));
            AvIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
            return IT.Build(AvIrBitmap);
        }

        memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        AvIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
        return IT.Build(AvIrBitmap);
    }

    private byte[] trackTag(Track track)
    {
        var imgs = TagLib.File.Create(track.Path).Tag.Pictures;
        return imgs[0].Data.Data;
    }
}
