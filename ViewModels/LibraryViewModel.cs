using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using AudioPlayer.Models;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class LibraryViewModel : ViewModelBase, INotifyPropertyChanged
{
    private MainWindowViewModel _window;
    private Library _library;
    private Library Library
    {
        get => _library;
        set
        {
            if (Equals(value, _library)) return;
            _library = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(Libs));
        }
    }
    public ObservableCollection<PlayList> Libs => _library.Playlists;
    
    public Interaction<CreatePlaylistViewModel, PlayList> ShowDialog { get; }

    public LibraryViewModel(PlayList playList, MainWindowViewModel MainWindow)
    {
        _window = MainWindow;
        ShowDialog = new Interaction<CreatePlaylistViewModel, PlayList>();
        // Check DB playlists;
        // if () {
        //
        // } else {
        _library = new();
        _library.Add(playList);
        _library.Add(new PlayList("a", "/Users/blazzeo/Downloads/Orange/"));
        foreach (var lib in Libs)
        {
            //Console.WriteLine(lib.Name);
        }
        // }
    }
    
    public void AddNewPlaylist()
    {
        CancellationToken token = new CancellationToken();
        string res = OpenFile(token).Result;
        
        PlayList newPlaylist = new PlayList("123", res); //CreatePlaylist();
        if (newPlaylist != null)
        {
            var lib = Library;
            lib.Add(newPlaylist);
            Library = lib;
        }
    }
    
    public ICommand CreateNewPlaylist { get; }
    
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
