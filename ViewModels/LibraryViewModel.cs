using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AudioPlayer.Models;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class LibraryViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _window;
    private Library _library;
    public Library Library
    {
        get => _library;
        set
        {
            if (Equals(value, _library)) return;
            this.RaiseAndSetIfChanged(ref _library, value);
            this.RaisePropertyChanged(nameof(Libs));
        }
    }

    public ObservableCollection<PlayList> Libs
    {
        get => _library.Playlists;
        set => _library.Playlists = value;
    }

    public LibraryViewModel(PlayList playList, MainWindowViewModel mainWindow)
    {
        _library = new Library();
        _window = mainWindow;
        
        Library.Add(playList);
    }
    
    public void AddNewPlaylist(PlayList? newPlaylist)
    {
        if (newPlaylist != null)
        {
            var lib = Library;
            lib.Add(newPlaylist);
            Library = lib;
        }

        _window.ToLibrary();
    }

    private int _id;
    
    public void Edit(PlayList editedPlaylist)
    {
        _id = Library.IndexOf(editedPlaylist);
        _window.OpenEditDialog(editedPlaylist);
    }

    public void UpdatePlaylist(PlayList newPlaylist)
    {
        Library.Playlists[_id] = newPlaylist;
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

    // public new event PropertyChangedEventHandler? PropertyChanged;
    //
    // private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    // {
    //     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    // }
    
    public async Task<IEnumerable<PlayList>> SearchAsync(string? query)
    {
        return Library.Playlists.Where(playlist =>
                playlist.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
