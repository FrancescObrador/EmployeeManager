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
    }
}
