using System.Collections.ObjectModel;

namespace AudioPlayer.Models;

public class Library
{
  private ObservableCollection<PlayList> playlistCollection;

  public Library(ObservableCollection<PlayList>? playLists = null)
  {
    playlistCollection = (playLists == null ? new() : playLists);
  }

  public ObservableCollection<PlayList> Playlists { get => playlistCollection; }

  public void Add(PlayList playList)
  {
    playlistCollection.Add(playList);
  }

  public void Remove(PlayList playList)
  {
    playlistCollection.Remove(playList);
  }
}
