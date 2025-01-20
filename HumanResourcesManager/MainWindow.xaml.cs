using System.Diagnostics;
using System.Runtime.CompilerServices;
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
using DataAccess;
using ExportPDF;
using HumanResourcesManager.Utilities;
using HumanResourcesManager.View;
using Microsoft.Win32;

namespace HumanResourcesManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetUp();
        }

        private void SetUp()
        {
            // Mover esto a la toolbox
            //InitializeAsync();

            this.mainFrame.Navigate(new EmployeesPage());
            Config.Load();
            SetCulture();
            this.Languages.SelectedItem = Languages.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Tag.ToString() == Config.Get("lang"));
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

        private void btnPageOne_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new EmployeesPage());
        }

        private void btnPageTwo_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new ProjectsPage());
        }

        private void btnPageThree_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new DepartmentsPage());
        }

        private void Languages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var langCode = (Languages.SelectedItem as ComboBoxItem)?.Tag.ToString();
            
            if(langCode == null)
            {
                return;
            }

            Config.Set("lang", langCode);
            Config.Save();

            SetCulture();
        }


        private void SetCulture()
        {
            var lang = Config.Get("lang");
            if (lang != null)
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
            }
            this.UpdateMainWindowUI();
        }

        private void RefreshFrame()
        {
            if (this.mainFrame?.Content is Page currentPage)
            {
                var pageType = currentPage.GetType(); 
                this.mainFrame.Navigate(null); 
                this.mainFrame.Navigate(Activator.CreateInstance(pageType));
            }
        }

        private void UpdateMainWindowUI()
        {
            if (this.Content != null)
            {
                this.lblLanguages.Content = Properties.Resources.Languages;
                this.btnPageOne.Content = Properties.Resources.employeesTitle;
                this.btnPageTwo.Content = Properties.Resources.projectsTitle;
                this.btnPageOne.Content = Properties.Resources.employeesTitle;
                this.btnGenerateDatabase.Content = Properties.Resources.generateDatabase;

                foreach (var item in this.Languages.Items.OfType<ComboBoxItem>())
                {
                    var langKey = item.Tag.ToString();
                    var resourceValue = Properties.Resources.ResourceManager.GetString(langKey, Properties.Resources.Culture);
                    if (resourceValue != null)
                    {
                        item.Content = resourceValue;
                    }
                }
            }
            this.RefreshFrame();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RefreshFrame();
        }

        private void btnGenerateDatabase_Click(object sender, RoutedEventArgs e)
        {
            InitializeAsync();
        }

        private void btnPDF_Click(object sender, RoutedEventArgs e)
        {
            var a = new Payroll(1, DateTime.Now, 20000, 2000, 1800, "Paco", "Arquitecto");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo PDF|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                PayrollPDFService provinciasPDF = new PayrollPDFService();
                if (provinciasPDF.SavePDF(a, saveFileDialog.FileName))
                {
                    if (MessageBox.Show("Datos exportados a PDF en " + saveFileDialog.FileName + "\n\n ¿Deseas abrirlo?",
                        "Exportación correcta", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        var p = new Process();
                        p.StartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                        {
                            UseShellExecute = true
                        };
                        p.Start();
                    }
                }
            }
        }
    }
}