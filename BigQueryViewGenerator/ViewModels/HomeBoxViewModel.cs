using BigQueryViewGenerator.Models;
using R3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BigQueryViewGenerator.ViewModels
{
    public class HomeBoxViewModel : MenuViewModelBase
    {
        public ReactiveCommand<Unit> CopySchemaSQLCommand { get; }
        public ReactiveCommand<Unit> CopySampleSQLCommand { get; }
        public ReactiveCommand<Unit> PasteSchemaCommand { get; }
        public ReactiveCommand<Unit> InsertPrevCommand { get; }
        public ReactiveCommand<Unit> InsertNextCommand { get; }
        public ReactiveCommand<Unit> RemoveSelectedCommand { get; }
        public BindableReactiveProperty<string> SQLText { get; }
        public BindableReactiveProperty<Column> SelectedColumn { get; }
        public ICSharpCode.AvalonEdit.Document.TextDocument SQLDocument { get; }
        public CreateSQLParameter Parameter { get; }
        public HomeBoxViewModel(MainWindowViewModel parent) : base(parent)
        {
            this.Parameter = new CreateSQLParameter(this.Config);

            this.SelectedColumn = new BindableReactiveProperty<Column>();
            this.InsertNextCommand = this.SelectedColumn.Select(x => x != null).ToReactiveCommand<Unit>();
            this.InsertNextCommand.Subscribe(_ =>
            {
                this.Parameter.InsertColumnNext(this.SelectedColumn.Value);
            });

            this.InsertPrevCommand = this.SelectedColumn.Select(x => x != null).ToReactiveCommand<Unit>();
            this.InsertPrevCommand.Subscribe(_ =>
            {
                this.Parameter.InsertColumnPrev(this.SelectedColumn.Value);
            });
            this.RemoveSelectedCommand = this.SelectedColumn.Select(x => x != null).ToReactiveCommand<Unit>();
            this.RemoveSelectedCommand.Subscribe(_ => this.Parameter.Columns.Remove(this.SelectedColumn.Value));

            this.CopySchemaSQLCommand = this.Parameter.CanCreateSchemaSQL.ToReactiveCommand();
            this.CopySchemaSQLCommand.Subscribe(_ => Clipboard.SetText(this.Parameter.GetSchemaSQL()));

            this.CopySampleSQLCommand = this.Parameter.CanCreateSampleSQL.ToReactiveCommand();
            this.CopySampleSQLCommand.Subscribe(_ => Clipboard.SetText(this.Parameter.GetSampleSQL()));

            this.PasteSchemaCommand = new ReactiveCommand<Unit>();
            this.PasteSchemaCommand.Subscribe(_ =>
            {
                this.Parameter.PropertyChanged -= Parameter_PropertyChanged;
                this.Parameter.SetColumns(Clipboard.GetText());
                this.Parameter.PropertyChanged += Parameter_PropertyChanged;
            });
            this.SQLDocument = new ICSharpCode.AvalonEdit.Document.TextDocument();
            this.Parameter.SQLText.Subscribe(_ => this.SQLDocument.Text = this.Parameter.SQLText.Value);
        }

        private void Parameter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SQLDocument.Text = this.Parameter.SQLText.Value;
        }
    }
}
