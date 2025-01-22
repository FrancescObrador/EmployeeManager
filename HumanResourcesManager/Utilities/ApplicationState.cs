using HumanResourcesManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.Utilities
{
    /// <summary>
    /// Global state of the application
    /// Used to preserve information between different pages
    /// </summary>
    public class ApplicationState
    {
        private static readonly ApplicationState _instance = new ApplicationState();
        public static ApplicationState Instance => _instance;

        private ApplicationState() { }

        public Employee? SelectedEmployee { get; set; }
        public Department? SelectedDepartment { get; set; }
        public Project? SelectedProject { get; set; }

    }
}
