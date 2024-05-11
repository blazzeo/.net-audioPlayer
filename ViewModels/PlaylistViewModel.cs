using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Models.TreeDataGrid;
using System.Collections.Generic;
using AudioPlayer.Models;
using System;
using System.IO;
using AudioPlayer.DataTemplate;
using Avalonia.Media.Imaging;

namespace AudioPlayer.ViewModels;

public class PlayListViewModel : ViewModelBase
{
    private MainWindowViewModel _window;
    private PlayList _playList;
    private readonly List<TrackInfo> _trackList;
    public string Title { get; }
    public FlatTreeDataGridSource<TrackInfo> AudioSource { get; }

    public PlayListViewModel(MainWindowViewModel window)
    {
        _window = window;
        _playList = new();
        _trackList = new List<TrackInfo>();
        Title = "Empty";
        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_trackList);
    }
    
    public PlayListViewModel(PlayList playList, MainWindowViewModel window)
    {
        _window = window;
        _playList = playList;
        _trackList = new(_playList.TrackList);

        Title = playList.Name!;

        AudioSource = new FlatTreeDataGridSource<TrackInfo>(_trackList)
        {
            Columns = {
              new TemplateColumn<TrackInfo>("Cover", new FuncDataTemplate<TrackInfo>((a,e) => GetButton(a))),
              new TextColumn<TrackInfo, string>("Title", x => x.Title),
              new TextColumn<TrackInfo, string>("Artist", x => x.Artist),
              new TextColumn<TrackInfo, string>("Album", x => x.Album),
              new TextColumn<TrackInfo, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
          }
        };
    }

    private Control GetButton(TrackInfo track)
    {
        var it = new ImageButtonCommand();
        var img = GetImage(track);
        Tuple<PlayerViewModel, PlayList, TrackInfo, Control> tuple = new (_window.PlayerVm, _playList, track, img);
       
        // Console.WriteLine(track.Title);
        return it.Build(tuple);
    }
    
    private Control GetImage(TrackInfo track)
    {
        var it = new SongImageTemplate();
        if (track == null)
        {
            return it.Build(new Bitmap(new MemoryStream(System.IO.File.ReadAllBytes("Assets/default-audio.png"))));
        } else
        return it.Build(track.Image);
    }
}