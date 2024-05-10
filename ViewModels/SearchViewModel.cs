using System;
using Avalonia.Controls.Primitives;

namespace AudioPlayer.ViewModels;

public class SearchViewModel : ViewModelBase
{
  private ViewModelBase _focusContent;
  private MainWindowViewModel _mainWindow;

  public SearchViewModel(MainWindowViewModel mainWindow)
  {
    _mainWindow = mainWindow;
    _focusContent = mainWindow.LibraryVm;
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
    if (_mainWindow.LocaleEn)
    {
     _mainWindow.SwitchLang("ru-RU"); 
    }
    else
    {
      _mainWindow.SwitchLang("en-EN");  
    }
  }
}
