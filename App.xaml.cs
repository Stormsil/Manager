using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using Manager.MVVM.View;
using Manager.MVVM.ViewModel;
using Prism.Modularity;

namespace Manager
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Регистрация типов для внедрения зависимостей
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<ControlVMHomeView, ControlVMHomeViewModel>();
            containerRegistry.RegisterForNavigation<ControlVMConsoleView, ControlVMConsoleViewModel>();
            containerRegistry.RegisterForNavigation<ControlVMSettingsView, ControlVMSettingsViewModel>();
            containerRegistry.RegisterForNavigation<CreateVMNewView, CreateVMNewViewModel>();
            containerRegistry.RegisterForNavigation<CreateVMConsoleView, CreateVMConsoleViewModel>();
            containerRegistry.RegisterForNavigation<CreateVMRandomMacView, CreateVMRandomMacViewModel>();
            containerRegistry.RegisterForNavigation<DiscordBotView, DiscordBotViewModel>();
            containerRegistry.RegisterForNavigation<DBEditorView, DBEditorViewModel>();

            // Регистрация MainWindow и MainViewModel
            containerRegistry.Register<MainWindow>();
            containerRegistry.Register<MainViewModel>();
        }


        protected override Window CreateShell()
        {
            // Возвращаем главное окно приложения
            return ContainerLocator.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Регистрация модулей, если есть
        }
    }
}
