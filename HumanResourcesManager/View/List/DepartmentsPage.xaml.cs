using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using HumanResourcesManager.View.Detail;
using HumanResourcesManager.ViewModel.List;
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

        private void departmentsDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (departmentsDataGrid.SelectedItem is Department selectedDepartment)
            {
                ApplicationState.Instance.SelectedDepartment = selectedDepartment;
                var detailPage = new DepartmentDetailPage();

                NavigationService.Navigate(detailPage);
            }
        }
    }
}
