using ReactiveUI;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace NavigationBug;

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        FirstViewModel => App.ServiceProvider.GetRequiredService<FirstView>(),
        SecondViewModel => App.ServiceProvider.GetRequiredService<SecondView>(),
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}