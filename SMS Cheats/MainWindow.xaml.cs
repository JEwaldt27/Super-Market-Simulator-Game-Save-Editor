using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static SMS_Cheats.Constants;

namespace SMS_Cheats
{
    public partial class MainWindow : Window
    {
        private string saveFilePath = "";
        private string[] saveFileContents;

        private string DefaultSaveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "Nokta Games", "Supermarket Simulator");

        public MainWindow()
        {
            InitializeComponent();
        }

        #region UI Control Events
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnBrowseSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select Save File",
                Filter = "Save files (*.es3)|*.es3|All files (*.*)|*.*"
            };

            if (Directory.Exists(DefaultSaveFolder))
                openFileDialog.InitialDirectory = DefaultSaveFolder;

            if (openFileDialog.ShowDialog() == true)
            {
                saveFilePath = openFileDialog.FileName;
                ReadSaveFile();
            }
        }

        private void btnApplyChanges_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        private void btnAutoLoadSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(DefaultSaveFolder))
                return;

            var mostRecentSave = new DirectoryInfo(DefaultSaveFolder).GetFiles("*.es3").OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            if (mostRecentSave == null)
                return;

            saveFilePath = mostRecentSave.FullName;
            ReadSaveFile();
        }
        #endregion

        private void ReadSaveFile()
        {
            if (string.IsNullOrEmpty(saveFilePath) || !File.Exists(saveFilePath))
                return;

            saveFileContents = File.ReadAllLines(saveFilePath);

            txtLevel.Text = ReadValue<string>(Constants.SearchKeys.CurrentStoreLevel);
            txtMoney.Text = ReadValue<string>(Constants.SearchKeys.Money);
            txtCheckoutCount.Text = ReadValue<string>(Constants.SearchKeys.CompletedCheckoutCount);
            txtCurrentDay.Text = ReadValue<string>(Constants.SearchKeys.CurrentDay);
            txtStoreName.Text = ReadValue<string>(Constants.SearchKeys.StoreName);
        }

        private string GetUniqueFilePath(string filePath)
        {
            if (!File.Exists(filePath))
                return filePath;

            string directory = Path.GetDirectoryName(filePath);
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int counter = 1;
            string newFilePath;

            do
            {
                string newFilename = $"{filenameWithoutExt} ({counter}){extension}";
                newFilePath = Path.Combine(directory, newFilename);
                counter++;
            } while (File.Exists(newFilePath));

            return newFilePath;
        }

        private void SaveChanges()
        {
            var backupFilePath = Path.ChangeExtension(saveFilePath, "bak");
            backupFilePath = GetUniqueFilePath(backupFilePath);
            File.Copy(saveFilePath, backupFilePath, true);

            Apply<int>(Constants.SearchKeys.CurrentStoreLevel, txtLevel.Text, false);
            Apply<double>(Constants.SearchKeys.Money, txtMoney.Text, false);
            Apply<int>(Constants.SearchKeys.CompletedCheckoutCount, txtCheckoutCount.Text, false);
            Apply<int>(Constants.SearchKeys.CurrentDay, txtCurrentDay.Text, false);
            Apply<string>(Constants.SearchKeys.StoreName, txtStoreName.Text, false);

            if (chkMaxFuel.IsChecked == true)
                Apply<int>(Constants.SearchKeys.VehicleGasLevel, Constants.GameValues.MaxFuel, true);

            File.WriteAllLines(saveFilePath, saveFileContents);

            MessageBox.Show($"Changes applied successfully.\n\nA backup file was created at {backupFilePath}", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private T ReadValue<T>(string searchKey)
        {
            try
            {
                for (int i = 0; i < saveFileContents.Length; i++)
                {
                    if (saveFileContents[i].Contains(searchKey))
                    {
                        var foundValue = saveFileContents[i].Split(':').Last();
                        foundValue = foundValue.Replace("\"", ""); //Remove any extra quotes
                        foundValue = foundValue.Remove(foundValue.Length - 1, 1).Trim(); //Remove the last comma and trim any whitespace.
                        return (T)Convert.ChangeType(foundValue, typeof(T));
                    }
                }
                return default;
            }
            catch (Exception)
            {
                MessageBox.Show($"Error reading value for {searchKey}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return default;
            }
        }

        private void Apply<T>(string searchKey, object newValue, bool isArray)
        {
            if (string.IsNullOrEmpty(saveFilePath) || !File.Exists(saveFilePath))
                return;

            try
            {
                T typedValue = (T)Convert.ChangeType(newValue, typeof(T));
            }
            catch (Exception)
            {
                MessageBox.Show($"'{newValue}' is not valid for {searchKey}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                for (int i = 0; i < saveFileContents.Length; i++)
                {
                    if (saveFileContents[i].Contains(searchKey))
                    {
                        string valueToSave = string.Empty;

                        if (typeof(T) == typeof(string))
                            valueToSave = $"{searchKey} : \"{newValue}\",";
                        else 
                            valueToSave = $"{searchKey} : {newValue},";

                        saveFileContents[i] = valueToSave;

                        if (!isArray)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
