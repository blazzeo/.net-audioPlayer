using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AudioPlayer.Models;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class LibraryViewModel : ViewModelBase
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

    public LibraryViewModel(PlayList playList, MainWindowViewModel mainWindow)
    {
        _window = mainWindow;
        
        _library = new();
        _library.Add(playList);
    }
    
    public void AddNewPlaylist(PlayList newPlaylist)
    {
        if (newPlaylist != null)
        {
            var lib = Library;
            lib.Add(newPlaylist);
            Library = lib;
        }

        _window.ToLibrary();
    }

    public void EditPlaylist(PlayList editedPlaylist)
    {
        if ()
        {
            Library.
            _window.ToLibrary();
        }
    }

    public void DeletePlaylist(PlayList playList)
    {
        Library.Remove(playList);
        _window.ToLibrary();
    }

    public void SetPlaylist(PlayList playList)
    {
        _window.PlaylistVm = new PlayListViewModel(playList);
        _window.ToPlaylists();
    }

    public void Cancel()
    {
        _window.ToLibrary();
    }

    public void CreateNew()
    {
        _window.OpenCreateDialog();
    }

    public new event PropertyChangedEventHandler? PropertyChanged;
    
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
