using BigQueryViewGenerator.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using R3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        public ReactiveCommand<Unit> ExportCommand { get; }
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

            this.ExportCommand = this.Parameter.CanExport.ToReactiveCommand();
            this.ExportCommand.Subscribe(async _ =>
            {
                using(var dlg=new CommonOpenFileDialog())
                {
                    dlg.IsFolderPicker = true;
                    if(dlg.ShowDialog()== CommonFileDialogResult.Ok)
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        try
                        {
                            await new DocumentGenerator(this.Parameter).ExportAsync(dlg.FileName);
                        }
                        finally
                        {
                            Mouse.OverrideCursor = null;
                        }
                    }
                }
            });
        }

        private void Parameter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SQLDocument.Text = this.Parameter.SQLText.Value;
        }
    }
}
