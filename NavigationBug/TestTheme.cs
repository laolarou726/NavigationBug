using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using PropertyChanged;
using System;
using System.Collections.Generic;

namespace NavigationBug;

[DoNotNotify]
public class TestTheme : AvaloniaObject, IStyle, IResourceProvider
{
    private readonly Uri _baseUri;
    private Styles _fluentDark = new();
    private Styles _fluentLight = new();
    private bool _isLoading;
    private IStyle? _loaded;
    private Styles _sharedStyles = new();
    
    public TestTheme(Uri baseUri)
    {
        _baseUri = baseUri;
        InitStyles(baseUri);
    }
    
    public TestTheme(IServiceProvider serviceProvider)
    {
        var uri =
            ((IUriContext)serviceProvider.GetRequiredService(typeof(IUriContext))).BaseUri.GetLeftPart(UriPartial
                .Authority);
        _baseUri = new Uri(uri);
        InitStyles(_baseUri);
    }

    /// <summary>
    ///     Gets the loaded style.
    /// </summary>
    public IStyle Loaded
    {
        get
        {
            if (_loaded != null) return _loaded!;

            _isLoading = true;

            _loaded = new Styles
            {
                _sharedStyles, _fluentLight[0], _fluentLight[1], _fluentLight[2]
            };

            _isLoading = false;

            return _loaded!;
        }
    }

    public event EventHandler OwnerChanged
    {
        add
        {
            if (Loaded is IResourceProvider rp) rp.OwnerChanged += value;
        }
        remove
        {
            if (Loaded is IResourceProvider rp) rp.OwnerChanged -= value;
        }
    }

    public IResourceHost? Owner => (Loaded as IResourceProvider)?.Owner;

    void IResourceProvider.AddOwner(IResourceHost owner) => (Loaded as IResourceProvider)?.AddOwner(owner);

    void IResourceProvider.RemoveOwner(IResourceHost owner) => (Loaded as IResourceProvider)?.RemoveOwner(owner);

    bool IResourceNode.HasResources => (Loaded as IResourceProvider)?.HasResources ?? false;

    public SelectorMatchResult TryAttach(IStyleable target, object? host) => Loaded.TryAttach(target, host);

    IReadOnlyList<IStyle> IStyle.Children => _loaded?.Children ?? Array.Empty<IStyle>();

    public bool TryGetResource(object key, out object? value)
    {
        if (!_isLoading && Loaded is IResourceProvider p) return p.TryGetResource(key, out value);

        value = null;
        return false;
    }
    
    private void InitStyles(Uri baseUri)
    {
        _sharedStyles = new Styles
        {
            new StyleInclude(baseUri)
            {
                Source = new Uri("Assets/ColorTheme/AccentColors.axaml", UriKind.Relative)
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("Assets/ControlBaseResources.axaml", UriKind.Relative)
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("Assets/Style/Base.axaml", UriKind.Relative)
            }
        };

        _fluentLight = new Styles
        {
            // 不要修改这三个 Style 的顺序
            new StyleInclude(baseUri)
            {
                Source = new Uri("Assets/ColorTheme/Light.axaml", UriKind.Relative)
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("Assets/ColorTheme/BaseLight.axaml", UriKind.Relative)
            },
            new StyleInclude(baseUri)
            {
                Source = new Uri("Assets/ColorTheme/FluentControlResourcesLight.axaml", UriKind.Relative)
            }
        };
    }
}