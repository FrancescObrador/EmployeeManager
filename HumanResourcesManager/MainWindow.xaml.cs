using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HumanResourcesManager.Utilities;
using HumanResourcesManager.ViewModel;

namespace HumanResourcesManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public EmployeeViewModel vm;

        public MainWindow()
        {
            InitializeComponent();

            // Mover esto a la toolbox
            InitializeAsync();
            vm = new EmployeeViewModel();
            this.DataContext = vm;
            Config.Save();
        }

        private async void InitializeAsync()
        {
            var result = MessageBox.Show("¿Quieres regenerar la base de datos? Selecciona un archivo sql, este proyecto contiene build.sql. Esto puede tardar unos minutos.", "Regenerar Base de datos", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await InitializeDatabase(() =>
                {
                    MessageBox.Show("La base de datos ha sido inicializada correctamente.", "Todo listo", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        private async Task InitializeDatabase(Action onComplete)
        {
            DatabaseUtilities p = new DatabaseUtilities();
            await p.PopulateDB();
            onComplete?.Invoke();
        }
    }
}