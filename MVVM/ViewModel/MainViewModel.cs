using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Manager.MVVM.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            HomeViewCommand = new DelegateCommand(() => NavigateTo("HomeView"));
            ControlVMHomeCommand = new DelegateCommand(() => NavigateTo("ControlVMHomeView"));
            ControlVMConsoleCommand = new DelegateCommand(() => NavigateTo("ControlVMConsoleView"));
            ControlVMSettingsCommand = new DelegateCommand(() => NavigateTo("ControlVMSettingsView"));
            CreateVMNewCommand = new DelegateCommand(() => NavigateTo("CreateVMNewView"));
            CreateVMConsoleCommand = new DelegateCommand(() => NavigateTo("CreateVMConsoleView"));
            CreateVMRandomMacCommand = new DelegateCommand(() => NavigateTo("CreateVMRandomMacView"));
            DiscordBotViewCommand = new DelegateCommand(() => NavigateTo("DiscordBotView"));
            DBEditorViewCommand = new DelegateCommand(() => NavigateTo("DBEditorView"));

            NavigateTo("HomeView");
        }

        public DelegateCommand HomeViewCommand { get; }
        public DelegateCommand ControlVMHomeCommand { get; }
        public DelegateCommand ControlVMConsoleCommand { get; }
        public DelegateCommand ControlVMSettingsCommand { get; }
        public DelegateCommand CreateVMNewCommand { get; }
        public DelegateCommand CreateVMConsoleCommand { get; }
        public DelegateCommand CreateVMRandomMacCommand { get; }
        public DelegateCommand DiscordBotViewCommand { get; }
        public DelegateCommand DBEditorViewCommand { get; }

        private void NavigateTo(string viewName)
        {
            _regionManager.RequestNavigate("ContentRegion", viewName);
            IsControlVMSubMenuVisible = viewName.StartsWith("ControlVM");
            IsCreateVMSubMenuVisible = viewName.StartsWith("CreateVM");
        }

        private bool _isControlVMSubMenuVisible;
        public bool IsControlVMSubMenuVisible
        {
            get => _isControlVMSubMenuVisible;
            set => SetProperty(ref _isControlVMSubMenuVisible, value);
        }

        private bool _isCreateVMSubMenuVisible;
        public bool IsCreateVMSubMenuVisible
        {
            get => _isCreateVMSubMenuVisible;
            set => SetProperty(ref _isCreateVMSubMenuVisible, value);
        }
    }
}
