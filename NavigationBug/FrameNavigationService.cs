using Microsoft.Extensions.Options;

namespace NavigationBug;

public class FrameNavigationServiceOptions : AbstractFrameNavigationServiceOptions
{
}

public sealed class FrameNavigationService : AbstractFrameNavigationService
{
    public FrameNavigationService(IOptions<FrameNavigationServiceOptions> options) : base(options)
    {
    }
}