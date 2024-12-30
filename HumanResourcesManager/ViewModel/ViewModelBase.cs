using HumanResourcesManager.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HumanResourcesManager.ViewModel
{
    public abstract class ViewModelBase<T> : IViewModel<T> where T : class
    {
        private Logger logger;

        protected HumanResourcesManagerContext _context;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public ObservableCollection<T> Items { get; set; }

        public int Total => Items.Count;
        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Total));
        }

        public ViewModelBase()
        {
            logger = new Logger(typeof(T).ToString());
            _context = new HumanResourcesManagerContext();
            Items = new ObservableCollection<T>();
            Items.CollectionChanged += ItemsCollectionChanged;
            Load();
        }

        public void Load()
        {
            _context.Set<T>().Load();
            Items = _context.Set<T>().Local.ToObservableCollection();
            logger.LogInfo("Loaded a set of " + typeof(T).ToString());
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Create(T newItem)
        {
            try
            {
                _context.Set<T>().Add(newItem);
                _context.SaveChanges();
                Items.Add(newItem);
                SelectedItem = newItem;
            }
            catch (Exception ex)
            {
               HandleError(ex);
            }
        }

        public void Update(T item)
        {
            try
            {

                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
                
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public void Delete()
        {
            try
            {
                if (SelectedItem != null)
                {
                    _context.Set<T>().Remove(SelectedItem);
                    _context.SaveChanges();
                    Items.Remove(SelectedItem);
                    SelectedItem = default;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public void SaveChanges()
        {
            try
            {
                int saved = _context.SaveChanges();
            }
            catch (Exception ex)
            {

                HandleError(ex);
            }
        }

        protected virtual void HandleError(Exception ex)
        {
            logger.LogError(ex.Message);
        }

    }
}
