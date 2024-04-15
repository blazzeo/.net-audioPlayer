using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Models.TreeDataGrid;
using System.Collections.Generic;
using System.IO;
using AudioPlayer.Templates;
using AudioPlayer.Models;
using System;
using ATL;

namespace AudioPlayer.ViewModels;

public class PlayListViewModel : ViewModelBase
{
    private List<Track> trackList;
    public String title { get; }
    public FlatTreeDataGridSource<Track> AudioSource { get; }

    public PlayListViewModel(PlayList playList)
    {
        trackList = new(playList.TrackList);

        title = playList.Name!;

        AudioSource = new FlatTreeDataGridSource<Track>(trackList)
        {
            Columns = {
              new TemplateColumn<Track>("Cover", new FuncDataTemplate<Track>((a,e) => GetButton(GetImage(a)))),
              new TextColumn<Track, string>("Title", x => x.Title),
              new TextColumn<Track, string>("Artist", x => x.Artist),
              new TextColumn<Track, string>("Album", x => x.Album),
              new TextColumn<Track, string>("Duration", x => TimeSpan.FromSeconds(x.Duration).ToString(@"mm\:ss")),
          }
        };
    }

    private Control GetButton(Control img)
    {
        var IT = new ImageButtonTemplate();
        return IT.Build(img);
    }

    private Control GetImage(Track track)
    {
        MemoryStream memory;
        Avalonia.Media.Imaging.Bitmap AvIrBitmap;
        var IT = new SongImageTemplate();
        if (track != null && Path.GetExtension(track.Path) != ".wav")
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
