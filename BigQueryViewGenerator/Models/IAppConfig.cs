using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public interface IAppConfig
    {
        public List<DatasetPair> Datasets { get; }
        public List<ColumnConverter> Converters { get; }
        public ViewConfig ViewConfig { get; }
        public TestConfig TestConfig { get; }
        public ThemeConfig ThemeConfig { get; }
    }
}
