using Avalonia.Controls;
using Avalonia.ReactiveUI;
using PropertyChanged;

namespace NavigationBug
{
    [DoNotNotify]
    public partial class SecondView : ReactiveUserControl<SecondViewModel>
    {
        public SecondView()
        {
        }

        public SecondView(SecondViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
