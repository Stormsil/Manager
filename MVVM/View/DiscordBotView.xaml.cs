using System.Windows.Controls;
using System.Windows.Input;

namespace Manager.MVVM.View
{
    public partial class DiscordBotView : UserControl
    {
        public DiscordBotView()
        {
            InitializeComponent();
        }

        private void ConsoleInput_KeyDown(object sender, KeyEventArgs e)
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
