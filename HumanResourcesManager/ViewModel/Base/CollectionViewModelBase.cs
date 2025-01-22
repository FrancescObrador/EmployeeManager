using HumanResourcesManager.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HumanResourcesManager.ViewModel.Base
{
    public abstract class CollectionViewModelBase<T> : ICollectionViewModel<T> where T : class
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

        public CollectionViewModelBase()
        {
            logger = new Logger(typeof(T).ToString() + " CollectionViewModel");
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

        public async void Create(T newItem)
        {
            try
            {
                _context.Set<T>().Add(newItem);
                await _context.SaveChangesAsync();
                Items.Add(newItem);
                SelectedItem = newItem;
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public async void Update(T item)
        {
            try
            {

                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public async void Delete()
        {
            try
            {
                if (SelectedItem != null)
                {
                    _context.Set<T>().Remove(SelectedItem);
                    await _context.SaveChangesAsync();
                    Items.Remove(SelectedItem);
                    SelectedItem = default;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public async void Delete(T item)
        {
            try
            {
                _context.Set<T>().Remove(item);
                await _context.SaveChangesAsync();
                Items.Remove(item);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public async void SaveChanges()
        {
            try
            {
                int saved = await _context.SaveChangesAsync();
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
