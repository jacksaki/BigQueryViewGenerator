using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class ColumnConverter
    {
        [JsonInclude]
        [JsonPropertyName("order")]
        public int Order { get; private set; }
        [JsonInclude]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonInclude]
        [JsonPropertyName("template")]
        public string Template { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsReadonly => this.IsManual;
        [JsonIgnore]
        public bool IsManual { get; private set; } = false;
        [JsonIgnore]
        public string TemplateKey => $"template_{this.Order}";
        internal static ColumnConverter CreateNew(int order)
        {
            return new ColumnConverter()
            {
                IsManual = false,
                Name = "新規作成",
                Order = order,
                Template = "@Model.Name"
            };
        }
        internal static ColumnConverter CreateManualItem(int order)
        {
            return new ColumnConverter()
            {
                IsManual = true,
                Name = "マニュアル",
                Order = order,
                Template = string.Empty,
            };
        }
        public void Compile()
        {
            Engine.Razor.Compile(this.Template, this.TemplateKey, typeof(Column));
        }
        public string GetText(Column column)
        {
            if (this.IsManual)
            {
                return column.ManualText.Value;
            }
            return Engine.Razor.Run(this.TemplateKey, typeof(Column), column);
        }
    }
}
