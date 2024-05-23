// File: App.xaml.cs
using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using Manager.MVVM.View;
using Manager.MVVM.ViewModel;
using Manager.Core;
using Manager.Core.Models;
using Prism.Modularity;

namespace Manager
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<VMService>();
            containerRegistry.RegisterSingleton<SchedulerService>();
            containerRegistry.RegisterSingleton<CreateVMService>();

            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<ControlVMHomeView, ControlVMHomeViewModel>();
            containerRegistry.RegisterForNavigation<ControlVMSettingsView, ControlVMSettingsViewModel>();
            containerRegistry.RegisterForNavigation<CreateVMNewView, CreateVMNewViewModel>();
            containerRegistry.RegisterForNavigation<CreateVMRandomMacView, CreateVMRandomMacViewModel>();
            containerRegistry.RegisterForNavigation<DiscordBotView, DiscordBotViewModel>();

            containerRegistry.Register<MainWindow>();
            containerRegistry.Register<MainViewModel>();
        }

        protected override Window CreateShell()
        {
            return ContainerLocator.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }
    }
}
