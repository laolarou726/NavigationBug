using Avalonia.ReactiveUI;
using PropertyChanged;

namespace NavigationBug
{
    [DoNotNotify]
    public partial class FirstView : ReactiveUserControl<FirstViewModel>
    {
        public FirstView()
        {
            
        }

        public FirstView(FirstViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
