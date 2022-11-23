using ReactiveUI;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace NavigationBug;

public class MainWindowViewModel : ReactiveObject
{
    public ICommand GoNext
    {
        get
        {
            return new RelayCommand(() =>
            {
                var vm = _counter switch
                {
                    0 => nameof(FirstView),
                    1 => nameof(SecondView)
                };

                NS.NavigateTo(vm);

                
                _counter++;
                _counter %= 2;
            });
        }
    }

    // The command that navigates a user back.
    public RelayCommand GoBack
    {
        get
        {
            return new RelayCommand(() =>
            {
                NS.GoBack();
            });
        }
    }

    private int _counter;
    public FrameNavigationService NS { get; }

    public MainWindowViewModel(FrameNavigationService ns)
    {
        NS = ns;
    }
}