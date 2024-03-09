using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class CreateSQLParameter:INotifyPropertyChanged
    {
        private static List<string> _SQLColumnNames = null;
        private static IEnumerable<string> SQLColumnNames
        {
            get
            {
                if (_SQLColumnNames == null)
                {
                    _SQLColumnNames = typeof(Column).GetProperties()
                        .Select(x => x.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? string.Empty)
                        .Where(x => !string.IsNullOrEmpty(x)).ToList();
                }
                return _SQLColumnNames;
            }
        }

        private CompositeDisposable disposables = new CompositeDisposable();
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public AppConfig Config { get; }
        public ObservableCollection<Column> Columns { get; }
        public BindableReactiveProperty<string> SourceTableName { get; }
        public BindableReactiveProperty<Table> SourceTable { get; }
        public BindableReactiveProperty<Table> DestView { get; }
        public BindableReactiveProperty<string> Description { get; }
        public BindableReactiveProperty<string> SQLText { get; }
        public BindableReactiveProperty<bool> CanCreateSchemaSQL { get; }
        public BindableReactiveProperty<bool> CanCreateSampleSQL { get; }
        private void RaisePropertyChanged(string propertyName = "")
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CreateSQLParameter(AppConfig config) : base()
        {
            this.Config = config;
            this.SQLText = new BindableReactiveProperty<string>();
            this.SourceTableName = new BindableReactiveProperty<string>();
            this.Description = new BindableReactiveProperty<string>();
            this.SourceTable = new BindableReactiveProperty<Table>();
            this.DestView = new BindableReactiveProperty<Table>();
            this.SQLText = new BindableReactiveProperty<string>();

            this.Columns = new ObservableCollection<Column>();
            this.Columns.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (Column item in e.OldItems)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                        item.Dispose();
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (Column item in e.NewItems)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                        item.AddTo(disposables);
                    }
                }
                RaisePropertyChanged(nameof(Columns));
            };
            this.CanCreateSchemaSQL = this.SourceTableName.Select(x => x.GetCharCount('.').Between(1, 2)).ToBindableReactiveProperty();
            this.CanCreateSampleSQL = this.CanCreateSchemaSQL;

            this.SourceTableName.Subscribe(x =>
            {
                if (!this.CanCreateSchemaSQL.Value)
                {
                    return;
                }
                var table = Table.Create(x);
                this.SourceTable.Value = table;
                this.DestView.Value = table.CreateView(this.Config.Datasets);
                GenerateSQL();
            });
            this.Description.Subscribe(x =>
            {
                GenerateSQL();
            });
        }
        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            GenerateSQL();
        }

        private void GenerateSQL()
        {
            this.SQLText.Value = GetSQL();
        }
        public string GetSQL()
        {
            try
            {
                var columns = this.Columns.Where(x => x.IsExportTarget.Value);
                if (!columns.Any())
                {
                    return string.Empty;
                }
                var sb = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(this.Description.Value))
                {
                    sb.AppendLine($"--{this.Description.Value}");
                }
                var table = Table.Create(this.SourceTableName.Value);
                var view = table.CreateView(this.Config.Datasets);
                if (table == null)
                {
                    return string.Empty;
                }
                sb.AppendLine($"CREATE OR REPLACE VIEW {view.SchemaName}.{view.TableName} AS ");
                sb.AppendLine("(");
                sb.AppendLine("    SELECT");
                sb.AppendLine($"     {string.Join("\r\n    ,", columns.Select(x => x.CreateSQL())).TrimEnd()}");
                sb.AppendLine("    FROM");
                sb.AppendLine($"     {table.SchemaName}.{table.TableName}");
                sb.AppendLine(")");

                return sb.ToString();
            }
            catch 
            {
                return string.Empty;
            }
        }

        public void SetColumns(string json)
        {
            var columns = JsonSerializer.Deserialize<Column[]>(json);
            if (columns == null || !columns.Any())
            {
                return;
            }
            var defaultConverter = this.Config.Converters.First();
            this.Columns.Clear();
            foreach(var col in columns)
            {
                col.Initialize(defaultConverter);
                this.Columns.Add(col);
            }
            var firstCol = columns.First();
            this.SourceTableName.Value = $"{firstCol.TableSchema}.{firstCol.TableName}";
        }
        public string GetSampleSQL()
        {
            if (string.IsNullOrEmpty(this.SourceTableName.Value))
            {
                return string.Empty;
            }
            return $@"SELECT
 *
FROM
 {this.SourceTableName.Value}
LIMIT 10";
        }

        public string GetSchemaSQL()
        {
            if (string.IsNullOrEmpty(this.SourceTableName.Value))
            {
                return string.Empty;
            }
            return @$"SELECT
 {string.Join("\r\n,", SQLColumnNames)}
FROM
 {this.SourceTable.Value.SchemaName}.information_schema.columns
WHERE
TABLE_NAME = '{this.SourceTable.Value.TableName}'
ORDER BY
 ORDINAL_POSITION";
        }
        public void InsertColumnNext(Column selectedColumn)
        {
            var manualConverter = this.Config.Converters.Where(x => x.IsManual).FirstOrDefault();
            var index = this.Columns.IndexOf(selectedColumn) + 1;
            this.Columns.Insert(index, Column.CreateFromTemplate(selectedColumn, manualConverter));
        }
        public void InsertColumnPrev(Column selectedColumn)
        {
            var manualConverter = this.Config.Converters.Where(x => x.IsManual).FirstOrDefault();
            var index = this.Columns.IndexOf(selectedColumn);
            this.Columns.Insert(index, Column.CreateFromTemplate(selectedColumn, manualConverter));
        }
    }
}
