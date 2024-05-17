using System.Windows;
using System.Windows.Input;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace Manager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Привязка DataContext к MainViewModel, зарегистрированному в контейнере Prism
            DataContext = ContainerLocator.Container.Resolve<Manager.MVVM.ViewModel.MainViewModel>();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                try
                {
                    this.DragMove();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Error during DragMove: " + ex.Message);
                }
            }
        }


        private void ControlVM_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as Manager.MVVM.ViewModel.MainViewModel;
            if (viewModel != null)
            {
                viewModel.IsControlVMSubMenuVisible = true;
                viewModel.IsCreateVMSubMenuVisible = false;
            }
        }

        private void CreateVM_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as Manager.MVVM.ViewModel.MainViewModel;
            if (viewModel != null)
            {
                viewModel.IsCreateVMSubMenuVisible = true;
                viewModel.IsControlVMSubMenuVisible = false;
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
            else
            {
                DragMove();
            }
        }

        private void MenuButton_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as Manager.MVVM.ViewModel.MainViewModel;
            if (viewModel != null)
            {
                viewModel.IsControlVMSubMenuVisible = false;
                viewModel.IsCreateVMSubMenuVisible = false;
            }
        }
    }
}
