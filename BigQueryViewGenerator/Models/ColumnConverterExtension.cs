using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public static class ColumnConverterExtension
    {
        public static ColumnConverter CreateNew(this IEnumerable<ColumnConverter> converters)
        {
            var nextOrder = converters.Max(x => x.Order) + 1;
            return ColumnConverter.CreateNew(nextOrder);
        }
        public static ColumnConverter CreateManualItem(this IList<ColumnConverter> converters)
        {
            if (converters.Where(x => x.IsManual).Any())
            {
                return null;
            }
            var nextOrder = converters.Any() ? converters.Max(x => x.Order) + 1 : 1;
            return ColumnConverter.CreateManualItem(nextOrder);
        }
    }
}
