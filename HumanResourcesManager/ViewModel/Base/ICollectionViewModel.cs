using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.ViewModel.Base
{
    public interface ICollectionViewModel<T> : INotifyPropertyChanged where T : class
    {
        T SelectedItem { get; set; }
        ObservableCollection<T> Items { get; set; }
        int Total { get; }
        void Load();
        void Create(T newItem);
        void Update(T item);
        void Delete();
    }

}
