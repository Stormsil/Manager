using System.Windows.Controls; // Используем правильный UserControl
using Manager.Core;
using Manager.MVVM.ViewModel;

namespace Manager.MVVM.View
{
    public partial class ControlVMSettingsView : System.Windows.Controls.UserControl // Указываем полное пространство имен
    {
        public ControlVMSettingsView()
        {
            InitializeComponent();
            var schedulerService = new SchedulerService();
            var vmService = new VMService(schedulerService); // Создаем экземпляр VMService
            DataContext = new ControlVMSettingsViewModel(schedulerService, vmService); // Передаем vmService в конструктор
        }
    }
}
