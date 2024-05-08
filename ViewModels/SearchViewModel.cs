using System;
using System.Reactive.Linq;
using AudioPlayer.Models;
using Avalonia.Controls.Primitives;
using ReactiveUI;

namespace AudioPlayer.ViewModels;

public class SearchViewModel : ViewModelBase
{
  private ViewModelBase _focusContent;
  private readonly MainWindowViewModel _mainWindow;
  private string? _searchText;
  private bool _isBusy;

  public string? SearchText
  {
    get => _searchText;
    set => this.RaiseAndSetIfChanged(ref _searchText, value);
  }
  
  public bool IsBusy
  {
    get => _isBusy;
    set => this.RaiseAndSetIfChanged(ref _isBusy, value);
  }

  public SearchViewModel(MainWindowViewModel mainWindow)
  {
    _mainWindow = mainWindow;
    _focusContent = mainWindow.LibraryVm;
    IsBusy = false;
    // if (_focusContent.GetType() == typeof(PlayListViewModel))
    // {
    //   this.WhenAnyValue(x => x.SearchText)
    //     .Throttle(TimeSpan.FromMilliseconds(400))
    //     .ObserveOn(RxApp.MainThreadScheduler);
    //   // .Subscribe(DoSearchTrack!);
    // }
    // if (_focusContent.GetType() == typeof(LibraryViewModel))
    // {
    //   this.WhenAnyValue(x => x.SearchText)
    //     .Throttle(TimeSpan.FromMilliseconds(400))
    //     .ObserveOn(RxApp.MainThreadScheduler);
    //     // .Subscribe(DoSearchPlaylist!);
    // }
  }

  public ViewModelBase SearchContent { get => _focusContent;
    set => _focusContent = value;
  }

  public void SwitchToLibrary()
  {
    _mainWindow.ToLibrary();
  }

  public void SwitchToPlaylists()
  {
    _mainWindow.ToPlaylists();
  }

  public void SwitchLanguage()
  {
    _mainWindow.SwitchLang(_mainWindow.LocaleEn ? "ru-RU" : "en-EN");
  }

  // public async void DoSearchPlaylist(string? s)
  // {
  //   _isBusy = true;
  //   if (!string.IsNullOrWhiteSpace(s))
  //   {
  //     var playlists = await _focusContent.SearchAsync(s);
  //   }
  //   _isBusy = false;
  // }
  
  // public async void DoSearchTrack(string? s)
  // {
  //   _isBusy = true;
  //   if (!string.IsNullOrWhiteSpace(s))
  //   {
  //     var tracklist = await _focusContent.SearchAsync(s);
  //   }
  //   else
  //   {
  //     var tracklist = _focusContent.Content;
  //   }
  //   _isBusy = false;
  // }
}
