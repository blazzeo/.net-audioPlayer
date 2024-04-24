using System.Collections.ObjectModel;

namespace AudioPlayer.Models;

public class Library
{
  private ObservableCollection<PlayList> _playlistCollection;

  public Library(ObservableCollection<PlayList>? playLists = null)
  {
    _playlistCollection = (playLists == null ? new() : playLists);
  }

  public ObservableCollection<PlayList> Playlists { get => _playlistCollection; }

  public void Add(PlayList playList)
  {
    _playlistCollection.Add(playList);
  }

  public void Remove(PlayList playList)
  {
    _playlistCollection.Remove(playList);
  }
}
