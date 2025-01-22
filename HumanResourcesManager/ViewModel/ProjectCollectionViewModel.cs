using HumanResourcesManager.Model;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HumanResourcesManager.ViewModel.Base;

namespace HumanResourcesManager.ViewModel
{
    class ProjectCollectionViewModel : CollectionViewModelBase<Project>
    {
        public OxyPlot.PlotModel? PlotModel { get; private set; }

        public ProjectCollectionViewModel() : base()
        {
            GenerateChart();
        }

        private void GenerateChart()
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

            foreach (var group in Items)
            {
                seriesP1.Slices.Add(new PieSlice(group.name, group.EmployeeProjects.Count)
                {
                    IsExploded = false,
                    Fill = OxyColor.FromHsv(0.1 * seriesP1.Slices.Count, 0.7, 0.9),
                });
            }


            PlotModel.Series.Add(seriesP1);
        }
    }
}
