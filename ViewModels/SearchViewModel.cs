namespace AudioPlayer.ViewModels;

public class SearchViewModel : ViewModelBase
{
  private ViewModelBase? focusContent;

  public SearchViewModel() {
    focusContent = null;
  }

  public SearchViewModel(ViewModelBase content) {
    focusContent = content;
  }

}
