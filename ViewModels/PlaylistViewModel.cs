using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Models.TreeDataGrid;
using System.Collections.Generic;
using AudioPlayer.Templates;
using AudioPlayer.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using ATL;
using Avalonia.Media.Imaging;
using ReactiveUI;
using TagLib;

namespace AudioPlayer.ViewModels;

public class PlayListViewModel : ViewModelBase
{
    private PlayList _playList;
    private List<TrackInfo> _originalTracklist;
    private List<TrackInfo> _trackList;
    public List<TrackInfo> Content { get => _trackList; set => _trackList = value; }
    
    public string Title { get; }
    public FlatTreeDataGridSource<TrackInfo> AudioSource { get; }
    public Bitmap Image { get; }
    
    public List<TrackInfo> TrackList
    {
        get => _trackList;
        set => this.RaiseAndSetIfChanged(ref _trackList, value);
    }

    public PlayListViewModel(PlayList playList)
    {
        _playList = playList;
        TrackList = new(_playList.TrackList);
        _originalTracklist = new List<TrackInfo>(playList.TrackList);
        Image = playList.Image;

        Title = playList.Name!;

        AudioSource = new FlatTreeDataGridSource<TrackInfo>(TrackList)
        {
            Columns = {
              new TemplateColumn<TrackInfo>("Cover", new FuncDataTemplate<TrackInfo>((a,e) => GetButton(GetImage(a)))),
              new TextColumn<TrackInfo, string>("Title", x => x.Title),
              new TextColumn<TrackInfo, string>("Artist", x => x.Artist),
              new TextColumn<TrackInfo, string>("Album", x => x.Album),
              new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
          }
        };
    }

    private static Control GetButton(Control img)
    {
        var it = new ImageButtonTemplate();
        
        return it.Build(img);
    }

    private static Control GetImage(TrackInfo track)
    {
        var it = new SongImageTemplate();
        if (track == null)
        {
            return it.Build(new Bitmap(new MemoryStream(System.IO.File.ReadAllBytes("Assets/default-audio.png"))));
        } 
        else return it.Build(track.Image);
    }
}