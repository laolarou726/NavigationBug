using Avalonia.ReactiveUI;
using PropertyChanged;

namespace NavigationBug
{
    [DoNotNotify]
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
        }

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}