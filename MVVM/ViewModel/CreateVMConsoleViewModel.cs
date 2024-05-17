using Prism.Mvvm;
using System;
using System.Windows.Threading;

namespace Manager.MVVM.ViewModel
{
    public class CreateVMConsoleViewModel : BindableBase // Изменено с internal на public
    {
        private readonly Dispatcher _dispatcher;
        private string _consoleText;

        public CreateVMConsoleViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            ConsoleText = string.Empty;
        }

        public string ConsoleText
        {
            get => _consoleText;
            set => SetProperty(ref _consoleText, value);
        }

        public void AppendToConsole(string message)
        {
            _dispatcher.Invoke(() =>
            {
                ConsoleText += $"{DateTime.Now:G}: {message}{Environment.NewLine}";
            });
        }
    }
}
