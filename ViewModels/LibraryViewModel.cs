using System;
using System.Collections.ObjectModel;
using AudioPlayer.Models;

namespace AudioPlayer.ViewModels;

public class LibraryViewModel : ViewModelBase
{
    public Library library;
    public ObservableCollection<PlayList> libs { get => library.Playlists; }

    public LibraryViewModel(PlayList playList)
    {
        // Check DB playlists;
        // if () {
        //
        // } else {
        library = new();
        library.Add(playList);
        library.Add(new PlayList("a", "/Users/blazzeo/Downloads/Orange/"));
        foreach (var lib in libs)
        {
            Console.WriteLine(lib.Name);
        }
        // }
    }

    // public Control GetImage(Track track)
    // {
    //     MemoryStream memory;
    //     Avalonia.Media.Imaging.Bitmap AvIrBitmap;
    //     var IT = new ImageTemplate();
    //     if (track != null)
    //     {
    //         memory = new MemoryStream(trackTag(track));
    //         AvIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
    //         return IT.Build(AvIrBitmap);
    //     }
    //
    //     memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
    //     AvIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
    //     return IT.Build(AvIrBitmap);
    // }
    //
    // public byte[] trackTag(Track track)
    // {
    //     var imgs = TagLib.File.Create(track.Path).Tag.Pictures;
    //     return imgs[0].Data.Data;
    // }
}
