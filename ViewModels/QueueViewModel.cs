using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Controls.Models.TreeDataGrid;
using System.IO;
using AudioPlayer.Templates;
using System;
using System.Collections.ObjectModel;
using ATL;

namespace AudioPlayer.ViewModels;

public class QueueListViewModel : ViewModelBase
{
    private ObservableCollection<Track>? _activeTracklist;
    public FlatTreeDataGridSource<Track> ActiveTrackList { get; }

    public QueueListViewModel()
    {
        _activeTracklist = new();
    }

    public QueueListViewModel(ObservableCollection<Track> tracklist)
    {
        _activeTracklist = tracklist;

        ActiveTrackList = new FlatTreeDataGridSource<Track>(_activeTracklist)
        {
            Columns = {
              new TemplateColumn<Track>("Cover", new FuncDataTemplate<Track>((a,e) => GetButton(GetImage(a)))),
              new TextColumn<Track, string>("Title", x => x.Title)
          }
        };
    }

    private Control GetButton(Control img)
    {
        var it = new ImageButtonTemplate();
        return it.Build(img);
    }

    private Control GetImage(Track track)
    {
        MemoryStream memory;
        Avalonia.Media.Imaging.Bitmap avIrBitmap;
        var it = new SongImageTemplate();
        if (track != null && Path.GetExtension(track.Path) != ".wav")
        {
            memory = new MemoryStream(trackTag(track));
            avIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
            return it.Build(avIrBitmap);
        }

        memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
        avIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
        return it.Build(avIrBitmap);
    }

    private byte[] trackTag(Track track)
    {
        var imgs = TagLib.File.Create(track.Path).Tag.Pictures;
        return imgs[0].Data.Data;
    }
}
