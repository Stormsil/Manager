using Manager.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Manager.MVVM.View
{
    public partial class CreateVMNewView : System.Windows.Controls.UserControl
    {
        public CreateVMNewView()
        {
            InitializeComponent();
            DiskComboBox.SelectionChanged += DiskComboBox_SelectionChanged;
        }

        private void DiskComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CreateVMNewViewModel viewModel && DiskComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedDisk = selectedItem.Content.ToString().Trim();
                var cleanedDisk = selectedDisk?.Replace(":", "").Trim();
                viewModel.SetVmDirectory(cleanedDisk);
            }
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateVMNewViewModel viewModel)
            {
                string vmName = NameTextBox.Text;
                if (!string.IsNullOrWhiteSpace(vmName))
                {
                    try
                    {
                        viewModel.ProgressValue = 0;
                        viewModel.IsProgressBarVisible = true;
                        await viewModel.CreateVM(vmName);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "VM Creation Error");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Please enter a valid VM name.", "Input Error");
                }
            }
        }
    }
}
