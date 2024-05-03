using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml;
using AudioPlayer.Models;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;

namespace AudioPlayer.ViewModels;

public class CreatePlaylistViewModel : ViewModelBase
{
    private PlayList _playList;
    private LibraryViewModel _library;
    private string pathFolder;
    private string pathImage;
    public string Name { get; }
    public Bitmap CoverImage { get; private set; }
    
    public new event PropertyChangedEventHandler? PropertyChanged;

    public CreatePlaylistViewModel(LibraryViewModel Library)
    {
        CoverImage = DefaultCover();
        _library = Library;
        _playList = new PlayList();
        this.CloseWindowCommand = new RelayCommand<ICloseable>(this.CloseWindow);
    }

    public async void OpenFolder()
    {
        pathFolder = OpenFolder(CancellationToken.None).Result;
        Console.WriteLine(11);
    }

    private async void OpenImage()
    {
        pathImage = OpenFile(CancellationToken.None).Result;
        Console.WriteLine(pathImage);
        CoverImage = new Bitmap(new MemoryStream(File.ReadAllBytes(pathImage)));
    }

    public void Create()
    {
        var newPlaylist = new PlayList(Name, pathFolder, pathImage);
        _library.AddNewPlaylist(newPlaylist);
    }

    public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }
    
    private void CloseWindow(ICloseable window)
    {
        if (window != null)
        {
            ;
        }
    }
    
    public void Cancel()
    {
        
    }

    private Bitmap DefaultCover()
    {
        
            MemoryStream memory = new MemoryStream(File.ReadAllBytes("Assets/default-audio.png"));
            return new Bitmap(memory);
        
    }
}