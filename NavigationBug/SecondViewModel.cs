using ReactiveUI;
using System;

namespace NavigationBug;

public class SecondViewModel : ReactiveObject, IRoutableViewModel
{
    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public SecondViewModel(FrameNavigationService ns) => HostScreen = ns;
}