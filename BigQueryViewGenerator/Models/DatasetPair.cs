using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace BigQueryViewGenerator.Models
{
     public class DatasetPair
    {
        [JsonInclude]
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
        [JsonInclude]
        [JsonPropertyName("dest")]
        public string Dest { get; set; } = string.Empty;
    }
}
