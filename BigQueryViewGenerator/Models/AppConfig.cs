using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class AppConfig: IAppConfig
    {
        public static string Path => System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".conf");
        [JsonInclude]
        [JsonPropertyName("datasets")]
        public List<DatasetPair> Datasets { get; private set; } = new List<DatasetPair>();
        [JsonInclude]
        [JsonPropertyName("converters")]
        public List<ColumnConverter> Converters { get; private set; } = new List<ColumnConverter>();
        [JsonInclude]
        [JsonPropertyName("view_sheet")]
        public ViewConfig ViewConfig { get; protected set; }
        [JsonInclude]
        [JsonPropertyName("test_sheet")]
        public TestConfig TestConfig { get; protected set; }
        [JsonInclude]
        [JsonPropertyName("theme")]
        public ThemeConfig ThemeConfig { get; protected set; }

        public static AppConfig Load()
        {
            var config = JsonSerializer.Deserialize<AppConfig>(System.IO.File.ReadAllText(Path))!;
            var manualConverter = config.Converters.CreateManualItem();
            if (manualConverter != null)
            {
                config.Converters.Add(manualConverter);
            }
            return config;
        }
        public void Save()
        {
            this.Converters = this.Converters.Where(x => !x.IsManual).OrderBy(x=>x.Order).ToList();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            System.IO.File.WriteAllText(Path, JsonSerializer.Serialize<AppConfig>(this,options));
        }
        public void CompileAllConverters()
        {
            Parallel.ForEach(this.Converters.Where(x => !x.IsManual), (x) =>
            {
                x.Compile();
            });
        }
    }
}
