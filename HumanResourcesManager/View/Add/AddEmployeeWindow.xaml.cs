using HumanResourcesManager.Model;
using HumanResourcesManager.ViewModel.Add;
using System.Windows;

namespace HumanResourcesManager.View
{
    public partial class AddEmployeeWindow : Window
    {
        private AddEmployeeViewModel addEmployeeVM;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            addEmployeeVM = new AddEmployeeViewModel();
            LoadComboBoxes();
        }

        public void LoadComboBoxes()
        {
            var projectList = addEmployeeVM.GetProjects();
            ProjectComboBox.ItemsSource = projectList;

            var departmentList = addEmployeeVM.GetDepartments();
            DepartmentComboBox.ItemsSource = departmentList;
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {

            var selectedProjectId = (int)ProjectComboBox.SelectedValue;
            
            var selectedDepartmentId = (int)DepartmentComboBox.SelectedValue;
            var selectedDepartment = addEmployeeVM.GetDepartment(selectedDepartmentId);

            var newEmployee = new Employee
            {
                first_name = txtFirstName.Text,
                last_name = txtLastName.Text,
                email = txtEmail.Text,
                phone_number = txtPhoneNumber.Text,
                position = txtPosition.Text,
                hire_date = DateTime.Now,
                salary = txtSalary.Text == "" ? 0 : decimal.Parse(txtSalary.Text),
                EmployeeProjects = new List<EmployeeProject>
                {
                    new EmployeeProject
                    {
                        project_id = selectedProjectId,
                        start_date = DateTime.Now
                    } 
                }
            };

            newEmployee.department = selectedDepartment;

            addEmployeeVM.Create(newEmployee);
            this.Close();
        }
    }
}
