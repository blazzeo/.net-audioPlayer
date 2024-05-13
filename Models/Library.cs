using System.Collections.ObjectModel;

namespace AudioPlayer.Models;

public class Library(ObservableCollection<PlayList>? playLists = null)
{
    public ObservableCollection<PlayList> Playlists { get; } = playLists ?? [];

    public void Add(PlayList playList)
    {
        Playlists.Add(playList);
    }

    public void Remove(PlayList playList)
    {
        Playlists.Remove(playList);
    }

    public void Edit(PlayList playList, int id)
    {
        Playlists[id] = playList;
    }

}