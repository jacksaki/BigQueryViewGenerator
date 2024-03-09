using BigQueryViewGenerator.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using R3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Resources.ResXFileRef;

namespace BigQueryViewGenerator.ViewModels
{
    public class EditConverterBoxViewModel : MenuViewModelBase
    {
        public ObservableCollection<ColumnConverterViewModel> Converters { get; }
        public BindableReactiveProperty<ColumnConverterViewModel> SelectedConverter { get; }
        public BindableReactiveProperty<bool> IsChanged { get; }
        public ReactiveCommand<Unit> SaveCommand { get; }
        public ReactiveCommand<Unit> AddCommand { get; }
        public ReactiveCommand<Unit> RemoveSelectedCommand { get; }

        public EditConverterBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.Converters = new ObservableCollection<ColumnConverterViewModel>();
            this.IsChanged = new BindableReactiveProperty<bool>(false);
            foreach(var converter in this.Parent.Config.Converters)
            {
                var vm = new ColumnConverterViewModel(converter);
                vm.Name.Subscribe(_ => this.IsChanged.Value = true);
                vm.Template.Subscribe(_ => this.IsChanged.Value = true);
                vm.AddTo(disposables);
                this.Converters.Add(vm);
            }
            this.SelectedConverter = new BindableReactiveProperty<ColumnConverterViewModel>();

            this.AddCommand = new ReactiveCommand<Unit>();
            this.AddCommand.Subscribe(_ =>
            {
                var converter = this.Converters.Select(x => x.Parent).CreateNew();
                var vm = new ColumnConverterViewModel(converter);
                vm.Name.Subscribe(_ => this.IsChanged.Value = true);
                vm.Template.Subscribe(_ => this.IsChanged.Value = true);
                vm.AddTo(disposables);
                this.Converters.Add(vm);
            });

            this.RemoveSelectedCommand = this.SelectedConverter.Select(x => x != null).ToReactiveCommand<Unit>();
            this.RemoveSelectedCommand.Subscribe(_ =>
            {
                this.Converters.Remove(this.SelectedConverter.Value);
            });

            this.SaveCommand = this.IsChanged.Select(x => x).ToReactiveCommand<Unit>();
            this.SaveCommand.Subscribe(_ =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Parallel.ForEach(this.Converters, (vm) =>
                    {
                        vm.ApplyChanges();
                        if (vm.IsNew)
                        {
                            vm.Parent.Compile();
                        }
                    });
                    this.Config.Converters.Clear();
                    this.Config.Converters.AddRange(this.Converters.Select(x => x.Parent));
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            });
        }
    }
}
