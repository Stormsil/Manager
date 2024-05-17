using System.Windows.Controls;
using System.Windows.Input;
using WpfUserControl = System.Windows.Controls.UserControl;
using WpfKeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Manager.MVVM.View
{
    public partial class DiscordBotView : WpfUserControl
    {
        public DiscordBotView()
        {
            InitializeComponent();
            DataContext = new Manager.MVVM.ViewModel.DiscordBotViewModel();
            Console.WriteLine("DiscordBotView initialized.");
        }

        private void ConsoleInput_KeyDown(object sender, WpfKeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = DataContext as Manager.MVVM.ViewModel.DiscordBotViewModel;
                if (viewModel != null)
                {
                    viewModel.ExecuteConsoleCommand(ConsoleInput.Text);
                    ConsoleInput.Clear();
                }
            }
        }
    }
}
