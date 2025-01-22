using ExportPDF;
using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HumanResourcesManager.ViewModel.Detail
{
    public class EmployeeDetailViewModel
    {
        private Logger logger;

        public EmployeeDetailViewModel()
        {
            logger = new Logger("EmployeeDetailViewModel");

            logger.LogInfo("AddEmployeeViewModel created");
        }

        public void GeneratePDF()
        {
            var empleado = ApplicationState.Instance.SelectedEmployee;
            var payroll = empleado.Payrolls.LastOrDefault();

            var primer = new PayrollItem() { Amount = payroll.gross_salary / 12, Concept = "Salario base", IsDeduction = false };
            var segundo = new PayrollItem() { Amount = payroll.deductions / 12, Concept = "Contingencias comunes", IsDeduction = true };

            var payrollPDFData = new ExportPDF.Payroll(empleado.id, DateTime.Now, ApplicationState.Instance.CompanyName,
                empleado.FullName, empleado.position,
                "12345678A", new List<PayrollItem>() { primer, segundo }, empleado.phone_number);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo PDF|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                PayrollPDFService provinciasPDF = new PayrollPDFService();
                if (provinciasPDF.SavePDF(payrollPDFData, saveFileDialog.FileName))
                {
                    logger.LogInfo("AddEmployeeViewModel created PDF correctly");
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
