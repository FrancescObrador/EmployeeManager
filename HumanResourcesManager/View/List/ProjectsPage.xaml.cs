using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using HumanResourcesManager.View.Detail;
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
    /// Interaction logic for ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        private ProjectCollectionViewModel projectsVM;
        
        public ProjectsPage()
        {
            InitializeComponent();

            projectsVM = new ProjectCollectionViewModel();
            var projectViewSource = (CollectionViewSource)this.Resources["projectsViewSource"];
            projectViewSource.Source = projectsVM.Items;

            this.DataContext = projectsVM;
        }

        private void btnDeleteProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var project = button.DataContext as Project;
                if (project != null)
                {
                    this.projectsVM.Delete(project);
                }
            }
        }

        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            //var addDepartmentWindow = new AddDepartmentWindow();
            // addDepartmentWindow.ShowDialog();
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            this.projectsVM.SaveChanges();
        }

        private void projectsDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (projectsDataGrid.SelectedItem is Project selectedProject)
            {
                ApplicationState.Instance.SelectedProject = selectedProject;
                var detailPage = new ProjectDetailPage();

                NavigationService.Navigate(detailPage);
            }
        }
    }
}
