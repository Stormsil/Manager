using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using Manager.MVVM.View;
using Manager.MVVM.ViewModel;
using Manager.Core;
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
            containerRegistry.RegisterForNavigation<ControlVMSettingsView, ControlVMSettingsViewModel>();
            containerRegistry.RegisterForNavigation<CreateVMNewView, CreateVMNewViewModel>();
            containerRegistry.RegisterForNavigation<CreateVMRandomMacView, CreateVMRandomMacViewModel>();
            containerRegistry.RegisterForNavigation<DiscordBotView, DiscordBotViewModel>();

            // Регистрация MainWindow и MainViewModel
            containerRegistry.Register<MainWindow>();
            containerRegistry.Register<MainViewModel>();

            // Регистрация CreateVMService
            containerRegistry.RegisterSingleton<CreateVMService>();
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
