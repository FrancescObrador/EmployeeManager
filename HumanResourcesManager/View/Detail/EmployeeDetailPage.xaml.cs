using ExportPDF;
using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HumanResourcesManager.View
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeeDetailPage : Page
    {
        public EmployeeDetailPage()
        {
            InitializeComponent();
            DataContext = ApplicationState.Instance;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnPDF_Click(object sender, RoutedEventArgs e)
        {
            //TODO Mover a ViewModel
            var empleado = ApplicationState.Instance.SelectedEmployee;
            var payroll = empleado.Payrolls.LastOrDefault();

            var primer = new PayrollItem() { Amount = payroll.gross_salary/12, Concept = "Salario base", IsDeduction = false };
            var segundo = new PayrollItem() { Amount = payroll.deductions/12, Concept = "Contingencias comunes", IsDeduction = true };

            var a = new ExportPDF.Payroll(empleado.id, DateTime.Now, "Euryst SL",
                empleado.FullName, empleado.position,
                "41624302Y", 30, new List<PayrollItem>() { primer, segundo }, empleado.phone_number);

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
