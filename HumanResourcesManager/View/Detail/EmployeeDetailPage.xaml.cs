using ExportPDF;
using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using HumanResourcesManager.ViewModel.Detail;
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
        public EmployeeDetailViewModel employeeDetailVM = new EmployeeDetailViewModel(); 

        public EmployeeDetailPage()
        {
            InitializeComponent();
            //DataContext = ApplicationState.Instance;
            this.DataContext = employeeDetailVM;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnPDF_Click(object sender, RoutedEventArgs e)
        {
            employeeDetailVM.GeneratePDF();
        }
    }
   
}
