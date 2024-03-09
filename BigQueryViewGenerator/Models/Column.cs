using DocumentFormat.OpenXml.Spreadsheet;
using R3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class Column:INotifyPropertyChanged, IDisposable
    {
        [JsonInclude]
        [JsonPropertyName("TABLE_SCHEMA")]
        public string TableSchema { get; private set; } = string.Empty;
        [JsonInclude]
        [JsonPropertyName("TABLE_NAME")]
        public string TableName { get; private set; } = string.Empty;

        [JsonInclude]
        [JsonPropertyName("COLUMN_NAME")]
        public string Name { get; private set; } = string.Empty;

        [JsonInclude]
        [JsonPropertyName("DATA_TYPE")]
        public string DataType { get; private set; } = string.Empty;

        [JsonInclude]
        [JsonPropertyName("IS_NULLABLE")]
        private string IsNullableText { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsNullable => "YES".Equals(this.IsNullableText);
        [JsonInclude]
        [JsonPropertyName("ORDINAL_POSITION")]
        public int OrdinalPosition { get; private set; }
        [JsonIgnore]
        public BindableReactiveProperty<string> NewName { get; }
        [JsonIgnore]
        public BindableReactiveProperty<ColumnConverter> Converter { get; }
        [JsonIgnore]
        public BindableReactiveProperty<string> ManualText { get; }
        [JsonIgnore]
        public BindableReactiveProperty<bool> IsManual { get; }
        [JsonIgnore]
        public BindableReactiveProperty<bool> IsExportTarget { get; }

        private CompositeDisposable disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public static Column CreateFromTemplate(Column template,ColumnConverter converter)
        {
            var col = new Column()
            {
                TableSchema = template.TableSchema,
                TableName=template.TableName,
                DataType=string.Empty,
                IsNullableText="YES",
                Name=string.Empty,
                
            };

            col.Converter.Value = converter;
            col.IsExportTarget.Value = true;
            col.ManualText.Value = string.Empty;
            col.NewName.Value = "new_column";
            return col;
        }
        public void Initialize(ColumnConverter converter)
        {
            this.NewName.Value = this.Name.ToLower();
            this.Converter.Value = converter;
        }
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Column()
        {
            this.NewName = new BindableReactiveProperty<string>();
            this.NewName.AddTo(disposables);
            this.NewName.Subscribe(_ =>
            {
                RaisePropertyChanged(nameof(this.NewName));
            });
            this.ManualText = new BindableReactiveProperty<string>(string.Empty);
            this.ManualText.AddTo(disposables);
            this.ManualText.Subscribe(_ => {
                RaisePropertyChanged(nameof(this.ManualText));
            });

            this.IsExportTarget = new BindableReactiveProperty<bool>(true);
            this.IsExportTarget.AddTo(disposables);
            this.IsExportTarget.Subscribe(_ =>
            {
                RaisePropertyChanged(nameof(IsExportTarget));
            });

            this.Converter = new BindableReactiveProperty<ColumnConverter>();
            this.Converter.AddTo(disposables);
            this.Converter.Subscribe(_ => {
                RaisePropertyChanged(nameof(this.Converter));
            });

            this.IsManual = this.Converter.Select(x => x?.IsManual == true).ToBindableReactiveProperty();
            this.IsManual.AddTo(disposables);
        }
        public string CreateSQL()
        {
            return $"{this.Converter.Value.GetText(this)} AS {this.NewName.Value}";
        }
        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
