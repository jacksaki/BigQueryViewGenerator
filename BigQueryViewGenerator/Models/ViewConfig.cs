using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class ViewConfig
    {
        [JsonPropertyName("source_table_dataset_cell")]
        public string SourceTableDatasetCell { get; set; }
        [JsonPropertyName("source_table_name_cell")]
        public string SourceTableNameCell { get; set; }
        [JsonPropertyName("dest_view_dataset_cell")]
        public string DestViewDatasetCell { get; set; }
        [JsonPropertyName("dest_view_name_cell")]
        public string DestViewNameCell { get; set; }
        [JsonPropertyName("description_cell")]
        public string DescriptionCell { get; set; }
        [JsonPropertyName("no_formula")]
        public string NoFormula { get; set; }
        [JsonPropertyName("no_column")]
        public string NoColumn { get; set; }
        [JsonPropertyName("sql_start_row")]
        public int DetailStartRow { get; set; }
        [JsonPropertyName("detail_start_column")]
        public string SQLStartColumn { get; set; }
        [JsonPropertyName("sql_end_column")]
        public string SQLEndColumn { get; set; }
        [JsonPropertyName("converter_name_column")]
        public string ConverterNameColumn { get; set; }
        [JsonPropertyName("test_target_column")]
        public string TestTargetColumn { get; set; }
        [JsonPropertyName("test_target_formula")]
        public string TestTargetFormula { get; set; }
        [JsonPropertyName("test_result_column")]
        public string TestResultColumn { get; set; }
        [JsonPropertyName("test_result_formula")]
        public string TestResultFormula { get; set; }
        [JsonPropertyName("source_column_name_column")]
        public string SourceColumnNameColumn { get; set; }
        [JsonPropertyName("source_column_data_type_column")]
        public string SourceColumnDataTypeColumn { get; set; }
    }
}
