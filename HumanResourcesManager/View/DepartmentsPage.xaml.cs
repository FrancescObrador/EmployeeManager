using HumanResourcesManager.Model;
using HumanResourcesManager.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DepartmentsPage.xaml
    /// </summary>
    public partial class DepartmentsPage : Page
    {
        private DepartmentCollectionViewModel departmentsVM;

        public DepartmentsPage()
        {
            InitializeComponent();

            departmentsVM = new DepartmentCollectionViewModel();
            var departmentViewSource = (CollectionViewSource)this.Resources["departmentsViewSource"];
            departmentViewSource.Source = departmentsVM.Items;

            this.DataContext = departmentsVM;
        }

        private void btnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var department = button.DataContext as Department;
                if (department != null)
                {
                    this.departmentsVM.Delete(department);
                }
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            this.departmentsVM.SaveChanges();
        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            //var addDepartmentWindow = new AddDepartmentWindow();
            // addDepartmentWindow.ShowDialog();
        }
    }
}
