using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using Microsoft.EntityFrameworkCore;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using OxyPlot.Legends;
using System.Net.WebSockets;

namespace HumanResourcesManager.ViewModel
{
    public class EmployeesViewModel : ViewModelBase<Employee>
    {
        public OxyPlot.PlotModel? PlotModel { get; private set; }

        public EmployeesViewModel() : base() {
            GenerateChart();
        }

        public void GenerateChart()
        {
            this.PlotModel = new PlotModel();

            dynamic seriesP1 = new PieSeries
            {
                StrokeThickness = 0, 
                InsideLabelPosition = 0.6,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelColor = OxyColors.White,
                TextColor = OxyColors.Gray, 
                FontSize = 14,
                InnerDiameter = 0.4
            };

            var departmentGroups = this.Items
               .GroupBy(item => item.department.name)
               .Select(group => new
               {
                   DepartmentName = group.Key,
                   Count = group.Count()
               });

            foreach (var group in departmentGroups)
            {
                seriesP1.Slices.Add(new PieSlice(group.DepartmentName, group.Count)
                {
                    IsExploded = false,
                    Fill = OxyColor.FromHsv(0.1 * seriesP1.Slices.Count, 0.7, 0.9),
                });
            }

            PlotModel.Series.Add(seriesP1);
        }
    }
}
