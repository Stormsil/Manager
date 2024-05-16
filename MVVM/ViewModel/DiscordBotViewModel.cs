using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;
using Manager.Core;
using System;
using System.IO;

namespace Manager.MVVM.ViewModel
{
    public class DiscordBotViewModel : BindableBase
    {
        private bool _isBotRunning;
        private StringBuilder _consoleText;
        private string _consoleInput;
        private DiscordBotService _botService;

        public DiscordBotViewModel()
        {
            ToggleBotCommand = new DelegateCommand(async () => await OnToggleBot());
            _consoleText = new StringBuilder();
            _botService = new DiscordBotService();
            RedirectConsoleOutput();
        }

        public ICommand ToggleBotCommand { get; }

        public bool IsBotRunning
        {
            get => _isBotRunning;
            set => SetProperty(ref _isBotRunning, value);
        }

        public string ConsoleText
        {
            get => _consoleText.ToString();
            set
            {
                _consoleText.Clear();
                _consoleText.Append(value);
                RaisePropertyChanged();
            }
        }

        public string ConsoleInput
        {
            get => _consoleInput;
            set => SetProperty(ref _consoleInput, value);
        }

        private async Task OnToggleBot()
        {
            if (IsBotRunning)
            {
                await StopBot();
            }
            else
            {
                await StartBot();
            }
        }

        private async Task StartBot()
        {
            await _botService.StartAsync("");
            AppendConsole("Bot started.");
            IsBotRunning = true;
        }

        private async Task StopBot()
        {
            await _botService.StopAsync();
            AppendConsole("Bot stopped.");
            IsBotRunning = false;
        }

        private void AppendConsole(string text)
        {
            _consoleText.AppendLine(text);
            RaisePropertyChanged(nameof(ConsoleText));
        }

        public void ExecuteConsoleCommand(string command)
        {
            // Обработка команд из консоли
            AppendConsole($"> {command}");
            // Добавьте здесь обработку команд
        }

        private void RedirectConsoleOutput()
        {
            Console.SetOut(new TextBoxWriter(AppendConsole));
        }

        private class TextBoxWriter : TextWriter
        {
            private readonly Action<string> _output;

            public TextBoxWriter(Action<string> output)
            {
                _output = output;
            }

            public override void Write(char value)
            {
                _output(value.ToString());
            }

            public override void Write(string value)
            {
                _output(value);
            }

            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
