using System;
using System.IO;
using System.Windows;

namespace SMS_Cheats
{
    public partial class MainWindow : Window
    {
        private string saveFilePath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        // Apply Money Cheat
        private void ApplyMoney_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SMSMoneyInput.Text, out int result))
            {
                ApplyCheat(saveFilePath, "\"Money\"", result.ToString());
            }
            else
            {
                MessageBox.Show("Please enter a valid number for money.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Apply Level Cheat
        private void ApplyLevel_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SMSLevelInput.Text, out int result))
            {
                ApplyCheat(saveFilePath, "\"CurrentStoreLevel\"", result.ToString());
            }
            else
            {
                MessageBox.Show("Please enter a valid number for level.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Apply Completed Checkouts Cheat
        private void ApplyCC_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SMSccInput.Text, out int result))
            {
                ApplyCheat(saveFilePath, "\"CompletedCheckoutCount\"", result.ToString());
            }
            else
            {
                MessageBox.Show("Please enter a valid number for completed checkouts.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Select Save File
        private void SelectSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select Save File",
                Filter = "Save files (*.es3)|*.es3|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                saveFilePath = openFileDialog.FileName;
                MessageBox.Show("Save file selected successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Function to apply cheats
        private void ApplyCheat(string filePath, string searchKey, string newValue)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("No save file selected. Please select a save file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    string[] contents = File.ReadAllLines(filePath);
                    bool found = false;

                    for (int i = 0; i < contents.Length; i++)
                    {
                        if (contents[i].Contains(searchKey))
                        {
                            contents[i] = searchKey + ": " + newValue + ",";
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        MessageBox.Show($"Key '{searchKey}' not found in the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        File.WriteAllLines(filePath, contents);
                        MessageBox.Show("Successfully modified.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CloseBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TopBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) 
            {
                DragMove();
            }
        }
    }
}
