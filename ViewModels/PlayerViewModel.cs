using AudioPlayer.Models;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System.ComponentModel;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using ATL;
using NAudio.Wave;

namespace AudioPlayer.ViewModels;

public class PlayerViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly Player _player;
    private PlayList _playList;
    private List<Track> _trackList;
    private Track _activeTrack;
    private bool _looping;
    private Bitmap _coverImage;
    
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    public Bitmap CoverImage { 
        get => _coverImage;
        private set
        {
            if (Equals(_coverImage, value)) return;
            _coverImage = value;
            RaisePropertyChanged();
        }
    }
    
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public PlayerViewModel()
    {
        _trackList = new();
        _player = new();
        _player.TrackIsEnd += OnTrackEnd;
        _playList = new();
        _activeTrack = new();
        CoverImage = GetImage();
    }

    public PlayerViewModel(Track track)
    {
        _trackList = new();
        _activeTrack = track;
        CoverImage = GetImage(_activeTrack);
        _player = new Player();
        _player.TrackIsEnd += OnTrackEnd;
        _playList = new PlayList();
        _activeTrack = track;
    }
    
    private void OnTrackEnd(object sender, EventArgs e)
    {
        NextSong(_looping);
    }

    public void SetAlbum(string path)
    {
        _playList = new PlayList("af", path);
    }

    public void SetVolume(int value)
    {
        _player.Volume = value / 100;
    }

    public void Play(Track track = null)
    {
        if (track != null)
            _activeTrack = track;
        CoverImage = GetImage(_activeTrack);
        _player.PlayFile();
    }

    public void LoopAudio()
    {
        _looping = !_looping;
    }

    public void NextSong(bool looping = false)
    {
        if (!looping)
        {
            var nextSongId = (_trackList.IndexOf(_activeTrack) != _trackList.Count) ?
              _trackList.IndexOf(_activeTrack) + 1 : 0;
            _activeTrack = _trackList[nextSongId];
            _player.AudioFile = new AudioFileReader(_activeTrack.Path);
        }
        Play(_activeTrack);
    }

    public void PrevSong()
    {
        var prevSongId = (_trackList.IndexOf(_activeTrack) != 0) ?
          _trackList.IndexOf(_activeTrack) - 1 : _trackList.Count;
        _activeTrack = _trackList[prevSongId];
        Play(_activeTrack);
    }

    public void Shuffle()
    {
        _playList.ToggleShuffle();
    }

    public double TotalTime
    {
        get => _player.AudioFile.TotalTime.TotalSeconds;
    }
    public double Position
    {
        get => _player.AudioFile.Position;
    }

    public int CurrentVolume
    {
        get => _player.Volume;
    }

    private Bitmap GetImage(Track track = null)
    {
        MemoryStream memory;
        if (track != null && Path.GetExtension(track.Path) != ".wav")
        {
            memory = new MemoryStream(TrackTag(track));
            return new Bitmap(memory);
        }
        memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        return new Bitmap(memory);
    }

    private byte[] TrackTag(Track track)
    {
        var images = TagLib.File.Create(track.Path).Tag.Pictures;
        return images[0].Data.Data;
    }
}
