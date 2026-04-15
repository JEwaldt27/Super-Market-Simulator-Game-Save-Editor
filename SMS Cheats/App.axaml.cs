using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SMS_Cheats.Services;
using SMS_Cheats.ViewModels;
using SMS_Cheats.Views;

namespace SMS_Cheats;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var service = new SaveFileService();
            var viewModel = new MainWindowViewModel(service);
            var window = new MainWindow(viewModel);
            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
