using BigQueryViewGenerator.Models;
using MaterialDesignThemes.Wpf;
using R3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace BigQueryViewGenerator.ViewModels
{
    public class ViewConfigViewModel : ViewModelBase
    {
        public BindableReactiveProperty<string> SourceTableDatasetCell { get; }
        public BindableReactiveProperty<string> SourceTableNameCell { get; }
        public BindableReactiveProperty<string> DestViewDatasetCell { get; }
        public BindableReactiveProperty<string> DestViewNameCell { get; }
        public BindableReactiveProperty<string> DescriptionCell { get; }
        public BindableReactiveProperty<string> NoFormula { get; }
        public BindableReactiveProperty<string> NoColumn { get; }
        public BindableReactiveProperty<int> DetailStartRow { get; }
        public BindableReactiveProperty<string> SQLStartColumn { get; }
        public BindableReactiveProperty<string> SQLEndColumn { get; }
        public BindableReactiveProperty<string> ConverterNameColumn { get; }
        public BindableReactiveProperty<string> TestTargetColumn { get; }
        public BindableReactiveProperty<string> TestTargetFormula { get; }
        public BindableReactiveProperty<string> TestResultColumn { get; }
        public BindableReactiveProperty<string> TestResultFormula { get; }
        public BindableReactiveProperty<string> SourceColumnNameColumn { get; }
        public BindableReactiveProperty<string> SourceColumnDataTypeColumn { get; }
        public ViewConfig Config { get; }
        public ViewConfigViewModel(ViewConfig config) : base()
        {
            this.Config = config;
            this.SourceTableDatasetCell = new BindableReactiveProperty<string>(config.SourceTableDatasetCell);
            this.SourceTableNameCell = new BindableReactiveProperty<string>(config.SourceTableNameCell);
            this.DestViewDatasetCell = new BindableReactiveProperty<string>(config.DestViewDatasetCell);
            this.DestViewNameCell = new BindableReactiveProperty<string>(config.DestViewNameCell);
            this.DescriptionCell = new BindableReactiveProperty<string>(config.DescriptionCell);
            this.NoFormula = new BindableReactiveProperty<string>(config.NoFormula);
            this.NoColumn = new BindableReactiveProperty<string>(config.NoColumn);
            this.DetailStartRow = new BindableReactiveProperty<int>(config.DetailStartRow);
            this.SQLStartColumn = new BindableReactiveProperty<string>(config.SQLStartColumn);
            this.SQLEndColumn = new BindableReactiveProperty<string>(config.SQLEndColumn);
            this.ConverterNameColumn = new BindableReactiveProperty<string>(config.ConverterNameColumn);
            this.TestTargetColumn = new BindableReactiveProperty<string>(config.TestTargetColumn);
            this.TestTargetFormula = new BindableReactiveProperty<string>(config.TestTargetFormula);
            this.TestResultColumn = new BindableReactiveProperty<string>(config.TestResultColumn);
            this.TestResultFormula = new BindableReactiveProperty<string>(config.TestResultFormula);
            this.SourceColumnNameColumn = new BindableReactiveProperty<string>(config.SourceColumnNameColumn);
            this.SourceColumnDataTypeColumn = new BindableReactiveProperty<string>(config.SourceColumnDataTypeColumn);
        }
    }
}
