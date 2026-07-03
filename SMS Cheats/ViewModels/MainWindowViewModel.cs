using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SMS_Cheats.Models;
using SMS_Cheats.Services;
using static SMS_Cheats.Constants;

namespace SMS_Cheats.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ISaveFileService _service;
    private SaveFile? _saveFile;

    // Injected by the View so the ViewModel stays platform-agnostic
    public Func<Task<string?>>? BrowseForFile { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ApplyChangesCommand))]
    private bool _isSaveLoaded;

    [ObservableProperty]
    private string _saveFilePath = "No file loaded";

    [ObservableProperty]
    private string _storeLevel = string.Empty;

    [ObservableProperty]
    private string _storePoints = string.Empty;

    [ObservableProperty]
    private string _storeUpgradeLevel = string.Empty;

    [ObservableProperty]
    private string _money = string.Empty;

    [ObservableProperty]
    private string _checkoutCount = string.Empty;

    [ObservableProperty]
    private string _currentDay = string.Empty;

    [ObservableProperty]
    private string _storeName = string.Empty;

    [ObservableProperty]
    private bool _maxFuelChecked;

    [ObservableProperty]
    private bool _unlockLicensesChecked;

    [ObservableProperty]
    private bool _clearLoansChecked;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public MainWindowViewModel(ISaveFileService service)
    {
        _service = service;
    }

    [RelayCommand]
    private async Task BrowseSaveFile()
    {
        if (BrowseForFile == null) return;

        var path = await BrowseForFile();
        if (path != null)
            LoadFile(path);
    }

    [RelayCommand]
    private void AutoLoadSave()
    {
        var defaultFolder = GetDefaultSaveFolder();
        if (!Directory.Exists(defaultFolder))
        {
            StatusMessage = "Default save folder not found.";
            return;
        }

        var path = _service.FindMostRecentSave(defaultFolder);
        if (path == null)
        {
            StatusMessage = "No save files found.";
            return;
        }

        LoadFile(path);
    }

    [RelayCommand(CanExecute = nameof(IsSaveLoaded))]
    private void ApplyChanges()
    {
        try
        {
            var backupPath = _service.CreateBackup(_saveFile!.FilePath);

            _service.Apply<int>(_saveFile, SearchKeys.CurrentStoreLevel, StoreLevel, false);
            _service.Apply<int>(_saveFile, SearchKeys.CurrentStorePoint, StorePoints, false);
            _service.Apply<int>(_saveFile, SearchKeys.StoreUpgradeLevel, StoreUpgradeLevel, false);
            _service.Apply<double>(_saveFile, SearchKeys.Money, Money, false);
            _service.Apply<int>(_saveFile, SearchKeys.CompletedCheckoutCount, CheckoutCount, false);
            _service.Apply<int>(_saveFile, SearchKeys.CurrentDay, CurrentDay, false);
            _service.Apply<string>(_saveFile, SearchKeys.StoreName, StoreName, false);

            if (MaxFuelChecked)
                _service.Apply<int>(_saveFile, SearchKeys.VehicleGasLevel, GameValues.MaxFuel.ToString(), true);

            if (UnlockLicensesChecked)
                _service.ReplaceArray(_saveFile, SearchKeys.UnlockedLicenses, string.Join(",",
                    Enumerable.Range(GameValues.FirstLicenseId, GameValues.LastLicenseId - GameValues.FirstLicenseId + 1)));

            if (ClearLoansChecked)
            {
                _service.Apply<int>(_saveFile, SearchKeys.LoanTermLength, "-1", true);
                _service.Apply<int>(_saveFile, SearchKeys.LoanRemainingPayments, "-1", true);
                _service.Apply<bool>(_saveFile, SearchKeys.LoanTaken, "false", true);
            }

            _service.Save(_saveFile);
            StatusMessage = $"Saved. Backup: {Path.GetFileName(backupPath)}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }

    private void LoadFile(string path)
    {
        try
        {
            _saveFile = _service.Load(path);
            SaveFilePath = path;
            StoreLevel = _service.ReadValue<string>(_saveFile, SearchKeys.CurrentStoreLevel);
            StorePoints = _service.ReadValue<string>(_saveFile, SearchKeys.CurrentStorePoint);
            StoreUpgradeLevel = _service.ReadValue<string>(_saveFile, SearchKeys.StoreUpgradeLevel);
            Money = _service.ReadValue<string>(_saveFile, SearchKeys.Money);
            CheckoutCount = _service.ReadValue<string>(_saveFile, SearchKeys.CompletedCheckoutCount);
            CurrentDay = _service.ReadValue<string>(_saveFile, SearchKeys.CurrentDay);
            StoreName = _service.ReadValue<string>(_saveFile, SearchKeys.StoreName);
            IsSaveLoaded = true;
            StatusMessage = "Save file loaded.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading file: {ex.Message}";
        }
    }

    private static string GetDefaultSaveFolder()
    {
        if (OperatingSystem.IsWindows())
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData", "LocalLow", "Nokta Games", "Supermarket Simulator");

        // Linux: Steam/Proton saves can vary — user can browse manually
        return string.Empty;
    }
}
