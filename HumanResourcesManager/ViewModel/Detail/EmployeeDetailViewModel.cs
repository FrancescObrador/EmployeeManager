using ExportPDF;
using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
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

        public PlotModel? PlotModel { get; private set; }

        public Employee Employee { get; set; }


        public EmployeeDetailViewModel()
        {
            logger = new Logger("EmployeeDetailViewModel");
            logger.LogInfo("AddEmployeeViewModel created");

            this.Employee = ApplicationState.Instance.SelectedEmployee;

            GenerateChart();
            
        }

        private void GenerateChart()
        {
            if (this.Employee?.Payrolls == null || !this.Employee.Payrolls.Any())
            {
                MessageBox.Show("No hay datos de nóminas disponibles para este empleado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var model = new PlotModel
            {
                Title = $"Gross Salary Evolution for {Employee.FullName}"
            };

            var barSeries = new BarSeries
            {
                Title = "Gross Salary",
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };

            foreach (var payroll in Employee.Payrolls.OrderBy(p => p.pay_date))
            {
                barSeries.Items.Add(new BarItem { Value = (double)payroll.gross_salary });
                categoryAxis.Labels.Add(payroll.pay_date.ToString("MMMM yyyy")); 
            }

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0,
                Title = "Gross Salary (€)"
            };

            model.Series.Add(barSeries);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            this.PlotModel = model;
        }

        public void GeneratePDF()
        {
            var payroll = this.Employee.Payrolls.LastOrDefault();

            var primer = new PayrollItem() { Amount = payroll.gross_salary, Concept = "Salario base", IsDeduction = false };
            var segundo = new PayrollItem() { Amount = payroll.deductions, Concept = "Contingencias comunes", IsDeduction = true };

            var payrollPDFData = new ExportPDF.Payroll(this.Employee.id, DateTime.Now, ApplicationState.Instance.CompanyName,
                this.Employee.FullName, this.Employee.position,
                "12345678A", new List<PayrollItem>() { primer, segundo }, this.Employee.phone_number);

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
