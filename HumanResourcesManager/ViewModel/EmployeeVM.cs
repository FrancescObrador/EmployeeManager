using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HumanResourcesManager.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public ObservableCollection<Employee> Employees { get; set; }

        private HumanResourcesManagerContext _context;

        public EmployeeViewModel()
        {
            _context = new HumanResourcesManagerContext();
            Employees = new ObservableCollection<Employee>();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            _context.Employees.Load();
            Employees = _context.Employees.Local.ToObservableCollection();
        }

        private void AddEmployee()
        {
            throw new NotImplementedException();
        }

        private bool CanAddEmployee()
        {
            throw new NotImplementedException();
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee != null)
                Employees.Remove(SelectedEmployee);
        }

        private bool CanDeleteEmployee()
        {
            return SelectedEmployee != null;
        }

        private void UpdateEmployee()
        {
            throw new NotImplementedException();
        }

        private bool CanUpdateEmployee()
        {
            return SelectedEmployee != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
