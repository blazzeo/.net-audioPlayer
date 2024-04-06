using AudioPlayer.Models;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using ATL;

namespace AudioPlayer.ViewModels;

public partial class PlayerViewModel : ViewModelBase
{
    public Player player;
    private PlayList playList;
    private List<Track> _trackList;
    private Track activeTrack;
    private bool looping;
    public Bitmap coverImage{get;}

    public PlayerViewModel()
    {
        _trackList = new();
        player = new();
        playList = new();
        activeTrack = new();
    }

    public PlayerViewModel(Track track)
    {
        _trackList = new();
        activeTrack = track;
        coverImage = GetImage(activeTrack);
        player = new();
        playList = new();
        activeTrack = new();
    }

    public void SetAlbum(string path)
    {
        playList = new("af", "/Users/blazzeo/MF Doom - 2004 - Mm..Food/");
    }

    public void SetVolume(int value)
    {
        player.Volume = value / 100;
    }

    public void Play(Track track = null!)
    {
        if (track != null)
            activeTrack = track;
        // player.Open(activeTrack);
        player.Play();
    }

    public void LoopAudio()
    {
        looping = !looping;
    }

    public void NextSong()
    {
        if (!looping)
        {
            var nextSongId = (_trackList.IndexOf(activeTrack) != _trackList.Count) ?
              _trackList.IndexOf(activeTrack) + 1 : 0;
            activeTrack = _trackList[nextSongId];
        }
        Play(activeTrack);
    }

    public void PrevSong()
    {
        var prevSongId = (_trackList.IndexOf(activeTrack) != 0) ?
          _trackList.IndexOf(activeTrack) - 1 : 0;
    }

    public void Shuffle()
    {
        playList.ToggleShuffle();
    }

    public TimeSpan TotalTime { get => player.TotalTime; }
    public TimeSpan Position { get => player.Position; }
    public int CurrentVolume { get => player.Volume; }

    private Bitmap GetImage(Track track)
    {
        MemoryStream memory;
        if (track != null)
        {
            memory = new MemoryStream(trackTag(track));
            return new Avalonia.Media.Imaging.Bitmap(memory);
        }
        memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        return new Avalonia.Media.Imaging.Bitmap(memory);
    }

    private byte[] trackTag(Track track)
    {
        var imgs = TagLib.File.Create(track.Path).Tag.Pictures;
        return imgs[0].Data.Data;
    }
}
