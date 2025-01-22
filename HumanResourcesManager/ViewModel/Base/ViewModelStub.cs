using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HumanResourcesManager.ViewModel.Base
{
    public class ViewModelBaseStub<T> : ICollectionViewModel<T> where T : class
    {
        private T _selectedItem;

        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();

        public int Total => Items.Count;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Load()
        {
            // Simula una carga de datos
            Items.Add((T)Activator.CreateInstance(typeof(T)));
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Total));
        }

        public void Create(T newItem)
        {
            Items.Add(newItem);
            SelectedItem = newItem;
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Total));
        }

        public void Update(T item)
        {
            // Simula una actualización
            OnPropertyChanged(nameof(Items));
        }

        public void Delete()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                SelectedItem = default;
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(Total));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
