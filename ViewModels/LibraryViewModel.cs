using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AudioPlayer.Models;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class LibraryViewModel : ViewModelBase, INotifyPropertyChanged
{
    private MainWindowViewModel _window;
    private PlayList? _selectedAlbum;
    private Library _library;
    private Library Library
    {
        get => _library;
        set
        {
            if (Equals(value, _library)) return;
            _library = value;
            this.RaiseAndSetIfChanged(ref _library, value);
            RaisePropertyChanged(nameof(Libs));
        }
    }
    
    public PlayList? SelectedAlbum
    {
        get => _selectedAlbum;
        set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
    }
    
    public ObservableCollection<PlayList> Libs => _library.Playlists;
    
    public Interaction<CreatePlaylistViewModel, PlayList> ShowDialog { get; }

    public LibraryViewModel(PlayList playList, MainWindowViewModel MainWindow)
    {
        _window = MainWindow;
        ShowDialog = new Interaction<CreatePlaylistViewModel, PlayList>();

        // CreateNewPlaylist = ReactiveCommand.CreateFromTask(async () =>
        // {
        //     var creator = new CreatePlaylistViewModel(this);
        //     AddNewPlaylist(await ShowDialog.Handle(creator));
        // });
        
        _library = new();
        _library.Add(playList);
        _library.Add(new PlayList("a", "/Users/blazzeo/Downloads/Orange/", "Assets/soprano.jpeg"));
        _library.Add(new PlayList("1234", "/Users/blazzeo/Downloads/Orange/", "Assets/default-audio.png"));
    }
    
    public void AddNewPlaylist(PlayList newPlaylist)
    {
        if (newPlaylist != null)
        {
            var lib = Library;
            lib.Add(newPlaylist);
            Library = lib;
        }
    }

    public void SetPlaylist(PlayList playList)
    {
        // Console.WriteLine(1231);
        _window.PlaylistVm = new PlayListViewModel(playList);
    }

    public void Cancel()
    {
        _window.ToLibrary();
    }
    
    public void SubmitPlaylist(PlayList playList)
    {
        _library.Playlists.Add(playList);
        _window.ToLibrary();
    }
    
    public void CreateNew()
    {
        _window.OpenCreateDialog();
    }
    
    // public ICommand CreateNewPlaylist { get; }
    
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
