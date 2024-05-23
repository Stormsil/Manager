using System.Windows.Controls; // Используем правильный UserControl
using Manager.Core;
using Manager.MVVM.ViewModel;

namespace Manager.MVVM.View
{
    public partial class ControlVMHomeView : System.Windows.Controls.UserControl // Указываем полное пространство имен
    {
        public ControlVMHomeView()
        {
            InitializeComponent();
            var schedulerService = new SchedulerService();
            var vmService = new VMService(schedulerService); // Создаем экземпляр VMService
            DataContext = new ControlVMHomeViewModel(vmService, schedulerService); // Передаем vmService в конструктор
        }

        private void SimpleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void SimpleButton_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
