using System.Collections.ObjectModel;
using AudioPlayer.Models;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class LibraryViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _window;
    private PlayList? _selectedAlbum;
    private ObservableCollection<PlayList> _libs;
    public ObservableCollection<PlayList> Libs
    {
        get => _libs;
        set => this.RaiseAndSetIfChanged(ref _libs, value);
    }
    private Library _library;
    public Library Library
    {
        get => _library;
        private set
        {
            if (Equals(value, _library)) return;
            _library = value;
            this.RaiseAndSetIfChanged(ref _library, value);
            this.RaisePropertyChanged(nameof(Libs));
        }
    }

    public LibraryViewModel(MainWindowViewModel mainWindow)
    {
        _window = mainWindow;
        _library = new();
    }

    public LibraryViewModel(PlayList playList, MainWindowViewModel mainWindow)
    {
        _window = mainWindow;

        _library = new();
        _library.Add(playList);
        _libs = _library.Playlists;
    }

    public PlayList? SelectedAlbum
    {
        get => _selectedAlbum;
        set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
    }

    public void AddNewPlaylist(PlayList newPlaylist)
    {
        if (newPlaylist != null)
        {
            var lib = Library;
            lib.Add(newPlaylist);
            Library = lib;
        }
        Libs = Library.Playlists;

        _window.ToLibrary();
    }

    public void UpdatePlaylist(PlayList editedPlaylist, int id)
    {
        if (editedPlaylist != null)
        {
            Library.Edit(editedPlaylist, id);
        }
        Libs = Library.Playlists;
    }

    public void EditPlaylist(PlayList playlistToEdit)
    {
        _window.OpenEditDialog(playlistToEdit);
    }

    public void DeletePlaylist(PlayList playList)
    {
        Library.Remove(playList);
        Libs = Library.Playlists;
        _window.ToLibrary();
    }

    public void SetPlaylist(PlayList playList)
    {
        _window.PlaylistVm = new PlayListViewModel(playList, _window);
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
}
