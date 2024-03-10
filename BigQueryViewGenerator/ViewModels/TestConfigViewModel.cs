using BigQueryViewGenerator.Models;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.ViewModels
{
    public class TestConfigViewModel:ViewModelBase
    {
        public BindableReactiveProperty<int> StartRow { get; }
        public BindableReactiveProperty<int> RowSpan { get; }
        public BindableReactiveProperty<string> SourceFullTableNameCell { get; }
        public BindableReactiveProperty<string> DestFullViewNameCell { get; }
        public BindableReactiveProperty<string> SQLTitleColumn { get; }
        public BindableReactiveProperty<string> SQLColumn { get; }
        public BindableReactiveProperty<string> SourceColumnTitleColumn { get; }
        public BindableReactiveProperty<string> SourceColumnNameColumn { get; }
        public BindableReactiveProperty<string> ResultTitleColumn { get; }
        public BindableReactiveProperty<string> ResultColumn { get; }
        public TestConfig Config { get; }
        public TestConfigViewModel(TestConfig config)
        {
            this.Config = config;
            this.StartRow = new BindableReactiveProperty<int>(config.StartRow);
            this.RowSpan = new BindableReactiveProperty<int>(config.RowSpan);
            this.SourceFullTableNameCell = new BindableReactiveProperty<string>(config.SourceFullTableNameCell);
            this.DestFullViewNameCell = new BindableReactiveProperty<string>(config.DestFullViewNameCell);
            this.SQLTitleColumn = new BindableReactiveProperty<string>(config.SQLTitleColumn);
            this.SQLColumn = new BindableReactiveProperty<string>(config.SQLColumn);
            this.SourceColumnTitleColumn = new BindableReactiveProperty<string>(config.SourceColumnTitleColumn);
            this.SourceColumnNameColumn = new BindableReactiveProperty<string>(config.SourceColumnNameColumn);
            this.ResultTitleColumn = new BindableReactiveProperty<string>(config.ResultTitleColumn);
            this.ResultColumn = new BindableReactiveProperty<string>(config.ResultColumn);
        }
    }
}
