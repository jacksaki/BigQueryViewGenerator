using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class Table
    {
        public string SchemaName { get; private set; }
        public string TableName { get; private set; }
        public string ProjectName { get; private set; }
        private Table()
        {
        }
        public string FullName => $"{this.SchemaName}.{this.TableName}";
        private string GetViewName()
        {
            if (this.TableName.StartsWith("t_", StringComparison.OrdinalIgnoreCase))
            {
                return $"v_{this.TableName.Substring(2)}";
            }
            else
            {
                return $"v_{this.TableName}";
            }
        }
        public Table CreateView(IEnumerable<DatasetPair>pairs)
        {
            var schema = pairs.Where(x => x.Source.Equals(this.SchemaName)).FirstOrDefault()?.Dest ?? "";
            return Table.Create($"{schema}.{GetViewName()}");
        }
        public static Table Create(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }
            var s = tableName.Split('.');
            if (s.Length < 2 || s.Length > 3)
            {
                return null;
            }
            var table = new Table();
            if (s.Length == 3)
            {
                table.ProjectName = s[0];
                table.SchemaName = s[1];
                table.TableName = s[2];
            }else
            {
                table.ProjectName = string.Empty;
                table.SchemaName = s[0];
                table.TableName = s[1];
            }
            return table;
        }
    }
}
