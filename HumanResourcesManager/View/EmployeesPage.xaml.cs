using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using HumanResourcesManager.ViewModel;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private EmployeesViewModel employeesVM;

        public EmployeesPage()
        {
            InitializeComponent();

            employeesVM = new EmployeesViewModel();
            var employeeViewSource = (CollectionViewSource)this.Resources["employeeViewSource"];
            employeeViewSource.Source = employeesVM.Items;

            this.DataContext = employeesVM;
        }


        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            employeesVM.SaveChanges();
        }
    }
}
