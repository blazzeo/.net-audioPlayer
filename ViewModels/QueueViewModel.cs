using Avalonia.Controls;
using AudioPlayer.Templates;
using AudioPlayer.Models;
using System.Collections.ObjectModel;
using Avalonia.Controls.Chrome;
using Avalonia.Interactivity;

namespace AudioPlayer.ViewModels;

public class QueueListViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindow;
    private PlayList _playList;

    public string Title{ get => _playList.Name ?? "Empty"; }
    public ObservableCollection<TrackInfo>? ActiveTracklist { get; }

    public QueueListViewModel(MainWindowViewModel window)
    {
        _mainWindow = window;
        ActiveTracklist = new();
    }

    public QueueListViewModel(MainWindowViewModel window, PlayList playList, ObservableCollection<TrackInfo> tracklist)
    {
        _mainWindow = window;
        _playList = playList;
        ActiveTracklist = tracklist;
    }

    public void PlayNow(object sender, RoutedEventArgs args)
    {
        var obj = (DockPanel)sender;
        
        _mainWindow.PlayerVm = new PlayerViewModel();
    }
    
}
