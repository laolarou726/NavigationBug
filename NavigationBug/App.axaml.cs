using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.DependencyInjection;
using PropertyChanged;

namespace NavigationBug
{
    [DoNotNotify]
    public partial class App : Application
    {
        private static IHost _host = null!;
        public static IServiceProvider ServiceProvider => _host.Services;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            _host = HostBuilder().Build();
            _host.RunAsync();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static IHostBuilder HostBuilder()
        {
            var hostBuilder = Host
                .CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddScoped<MainWindow>();
                    services.AddScoped<MainWindowViewModel>();
                    services.AddScoped<FirstView>();
                    services.AddScoped<FirstViewModel>();
                    services.AddScoped<SecondView>();
                    services.AddScoped<SecondViewModel>();

                    services.Configure(new Action<FrameNavigationServiceOptions>(options =>
                    {
                        options.Configure<FirstViewModel>(nameof(FirstView));
                        options.Configure<SecondViewModel>(nameof(SecondView));
                    }));

                    services.AddSingleton<FrameNavigationService>();
                });

            return hostBuilder;
        }
    }
}