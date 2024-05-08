using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AudioPlayer.ViewModels;

namespace AudioPlayer.Models;

public class Library
{
  private ObservableCollection<PlayList> _playlistCollection;
  public ObservableCollection<PlayList> Playlists 
  { 
      get => _playlistCollection;
      set => _playlistCollection = value;
  }

  public List<PlayList> Content
  {
      get => new List<PlayList>(Playlists);
      set => Playlists = new ObservableCollection<PlayList>(value);
  }
  
  public Library(ObservableCollection<PlayList>? playLists = null)
  {
    _playlistCollection = (playLists == null ? new() : playLists);
  }

  public void Add(PlayList playList)
  {
      _playlistCollection.Add(playList);
  }

  public void Remove(PlayList playList)
  {
      _playlistCollection.Remove(playList);
  }

  public void Edit(PlayList playList, int id)
  {
      // if (_playlistCollection[id] != null)
      // {
          _playlistCollection[id] = playList;
      // }
  }

  public int IndexOf(PlayList playList)
  {
      return Playlists.IndexOf(playList);
  }
 
  
}
