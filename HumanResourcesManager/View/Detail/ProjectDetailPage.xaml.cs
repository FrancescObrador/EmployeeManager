using HumanResourcesManager.Utilities;
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

namespace HumanResourcesManager.View.Detail
{
    /// <summary>
    /// Interaction logic for ProjectDetailPage.xaml
    /// </summary>
    public partial class ProjectDetailPage : Page
    {
        public ProjectDetailPage()
        {
            InitializeComponent();
            DataContext = ApplicationState.Instance;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
