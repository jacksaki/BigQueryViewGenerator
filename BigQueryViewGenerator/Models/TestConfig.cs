using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class TestConfig
    {
        [JsonPropertyName("start_row")]
        public int StartRow { get; set; }
        [JsonPropertyName("row_span")]
        public int RowSpan { get; set; }
        [JsonPropertyName("source_full_table_name_cell")]
        public string SourceFullTableNameCell { get; set; }
        [JsonPropertyName("dest_full_view_name_cell")]
        public string DestFullViewNameCell { get; set; }
        [JsonPropertyName("sql_title_column")]
        public string SQLTitleColumn { get; set; }
        [JsonPropertyName("sql_column")]
        public string SQLColumn { get; set; }
        [JsonPropertyName("source_column_title_column")]
        public string SourceColumnTitleColumn { get; set; }
        [JsonPropertyName("source_column_name_column")]
        public string SourceColumnNameColumn { get; set; }
        [JsonPropertyName("result_title_column")]
        public string ResultTitleColumn { get; set; }
        [JsonPropertyName("result_column")]
        public string ResultColumn { get; set; }
    }
}
