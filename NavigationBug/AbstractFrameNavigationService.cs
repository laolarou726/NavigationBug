using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReactiveUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NavigationBug;

public class AbstractFrameNavigationServiceOptions
{
    public Dictionary<string, Type> Pages { get; } = new();

    public void Configure<T>(string key) where T : IRoutableViewModel
    {
        Pages[key] = typeof(T);
    }
}

public interface INavigationService
{
    string CurrentPageKey { get; }
    void GoBack();
    void NavigateTo(string pageKey);
    void NavigateTo(string pageKey, object parameter);
}

public interface IFrameNavigationService : INavigationService
{
    object Parameter { get; }
}

public abstract class AbstractFrameNavigationService : ReactiveObject, IFrameNavigationService, IScreen
{
    #region Fields

    private bool _canNavigateBackExecute;

    #endregion

    #region Properties

    protected ConcurrentDictionary<string, Type> PagesByKey { get; }
    public RoutingState Router { get; } = new RoutingState();
    public string CurrentPageKey { get; private set; }
    public object? Parameter { get; protected set; }

    #endregion

    #region Ctors and Methods

    protected AbstractFrameNavigationService(IOptions<AbstractFrameNavigationServiceOptions> options)
    {
        Router.NavigateBack.CanExecute.Subscribe(val =>
        {
            _canNavigateBackExecute = val;
        });

        PagesByKey = new ConcurrentDictionary<string, Type>(options.Value.Pages);
    }

    public void GoBack()
    {
        if (_canNavigateBackExecute)
            Router.NavigateBack.Execute();
    }

    public void NavigateTo(string pageKey)
    {
        NavigateTo(pageKey, null);
    }

    public void NavigateTo(string pageKey, object? parameter)
    {
        if (!PagesByKey.ContainsKey(pageKey))
            throw new ArgumentException();

        var vmType = PagesByKey[pageKey];
        var vm = (IRoutableViewModel)App.ServiceProvider.GetRequiredService(vmType);

        Router.Navigate.Execute(vm);

        Parameter = parameter;
        CurrentPageKey = pageKey;
    }

    #endregion
}