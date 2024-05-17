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
        private bool _isToggling;
        private StringBuilder _consoleText;
        private string _consoleInput;
        private DiscordBotService _botService;

        public DiscordBotViewModel()
        {
            ToggleBotCommand = new DelegateCommand(async () => await OnToggleBot());
            _consoleText = new StringBuilder();
            _botService = new DiscordBotService();
            RedirectConsoleOutput();
            AppendConsole("DiscordBotViewModel initialized.");
        }

        public ICommand ToggleBotCommand { get; }

        public bool IsBotRunning
        {
            get => _isBotRunning;
            set
            {
                if (_isBotRunning != value)
                {
                    AppendConsole($"IsBotRunning changed from {_isBotRunning} to {value}");
                    _isBotRunning = value;
                    RaisePropertyChanged(nameof(IsBotRunning));
                    AppendConsole($"IsBotRunning changed to: {_isBotRunning}");
                }
            }
        }

        public string ConsoleText
        {
            get => _consoleText.ToString();
            set
            {
                _consoleText.Clear();
                _consoleText.Append(value);
                RaisePropertyChanged();
                AppendConsole($"ConsoleText set to: {value}");
            }
        }

        public string ConsoleInput
        {
            get => _consoleInput;
            set => SetProperty(ref _consoleInput, value);
        }

        private async Task OnToggleBot()
        {
            AppendConsole("OnToggleBot invoked.");

            // Запрещаем повторный вызов при уже измененном состоянии
            if (_isToggling)
            {
                AppendConsole("Toggle action is already in progress.");
                return;
            }

            _isToggling = true;
            try
            {
                if (IsBotRunning)
                {
                    AppendConsole("Calling StartBot.");
                    await StartBot();
                }
                else
                {
                    AppendConsole("Calling StopBot.");
                    await StopBot();
                }
            }
            finally
            {
                _isToggling = false;
                AppendConsole("Toggle action completed.");
            }
        }

        private async Task StartBot()
        {
            try
            {
                AppendConsole("Starting bot...");
                await _botService.StartAsync(""); // Замените на ваш токен
                AppendConsole("Bot started.");
                _isBotRunning = true; // Прямое изменение поля, чтобы избежать повторного вызова SetProperty
                RaisePropertyChanged(nameof(IsBotRunning));
            }
            catch (Exception ex)
            {
                AppendConsole($"Error starting bot: {ex.Message}");
                _isBotRunning = false;
                RaisePropertyChanged(nameof(IsBotRunning));
            }
        }

        private async Task StopBot()
        {
            try
            {
                AppendConsole("Stopping bot...");
                await _botService.StopAsync();
                AppendConsole("Bot stopped.");
                _isBotRunning = false; // Прямое изменение поля, чтобы избежать повторного вызова SetProperty
                RaisePropertyChanged(nameof(IsBotRunning));
            }
            catch (Exception ex)
            {
                AppendConsole($"Error stopping bot: {ex.Message}");
            }
        }

        private void AppendConsole(string text)
        {
            _consoleText.AppendLine(text);
            RaisePropertyChanged(nameof(ConsoleText));
        }

        public void ExecuteConsoleCommand(string command)
        {
            // Проверка на команду clear
            if (command.Equals("clear", StringComparison.OrdinalIgnoreCase))
            {
                ClearConsole();
            }
            else
            {
                AppendConsole($"> {command}");
                // Добавьте здесь обработку других команд
            }
        }

        private void ClearConsole()
        {
            _consoleText.Clear();
            RaisePropertyChanged(nameof(ConsoleText));
            AppendConsole("Console cleared.");
        }


        private void RedirectConsoleOutput()
        {
            Console.SetOut(new TextBoxWriter(AppendConsole));
            Console.SetError(new TextBoxWriter(AppendConsole)); // Перенаправление ошибок также в консоль приложения
            AppendConsole("Console output redirected.");
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
