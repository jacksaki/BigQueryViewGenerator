using BigQueryViewGenerator.Models;
using R3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BigQueryViewGenerator.ViewModels
{
    public class EditDatasetBoxViewModel : MenuViewModelBase
    {
        public ObservableCollection<DatasetViewModel> DatasetPairs { get; }
        public ReactiveCommand<Unit> AddCommand { get; }
        public ReactiveCommand<Unit> SaveCommand { get; }
        public ReactiveCommand<Unit> RemoveSelectedCommand { get; }
        public BindableReactiveProperty<DatasetViewModel> SelectedDatasetPair { get; }
        public EditDatasetBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.DatasetPairs = new ObservableCollection<DatasetViewModel>();
            this.DatasetPairs.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (DatasetViewModel item in e.OldItems)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                        item.Dispose();
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (DatasetViewModel item in e.NewItems)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                        item.Dispose();
                    }
                }
            };
            this.SelectedDatasetPair = new BindableReactiveProperty<DatasetViewModel>();
            this.AddCommand = new ReactiveCommand<Unit>();
            this.AddCommand.Subscribe(_ =>
            {
                this.DatasetPairs.Add(new DatasetViewModel(new DatasetPair() { Source = "new_source", Dest = "new_dest" }));
            });
            this.RemoveSelectedCommand = this.SelectedDatasetPair.Select(x => x != null).ToReactiveCommand();
            this.RemoveSelectedCommand.Subscribe(_ =>
            {
                this.DatasetPairs.Remove(this.SelectedDatasetPair.Value);
            });
            foreach (var datasetPair in this.Config.Datasets)
            {
                this.DatasetPairs.Add(new DatasetViewModel(datasetPair));
            }
            this.SaveCommand = new ReactiveCommand<Unit>();
            this.SaveCommand.Subscribe(_ =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Parallel.ForEach(this.DatasetPairs, (vm) =>
                    {
                        vm.ApplyChanges();
                    });
                    this.Config.Datasets.Clear();
                    this.Config.Datasets.AddRange(this.DatasetPairs.Select(x => x.Parent));
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            });
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
