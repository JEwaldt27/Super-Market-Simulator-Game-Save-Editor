using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SMS_Cheats.ViewModels;

namespace SMS_Cheats.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
        viewModel.BrowseForFile = BrowseForFileAsync;
    }

    private async Task<string?> BrowseForFileAsync()
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Save File",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Save files") { Patterns = ["*.es3"] },
                new FilePickerFileType("All files") { Patterns = ["*.*"] }
            ]
        });

        return files.Count > 0 ? files[0].Path.LocalPath : null;
    }

    private void TopBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }

    private void btnClose_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
