using BigQueryViewGenerator.Models;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.ViewModels
{
    public class DatasetViewModel:ViewModelBase
    {
        public DatasetPair Parent { get; }
        public BindableReactiveProperty<string> Source { get; }
        public BindableReactiveProperty<string> Dest { get; }
        public DatasetViewModel(DatasetPair pair)
        {
            this.Parent = pair;
            this.Source = new BindableReactiveProperty<string>(pair.Source);
            this.Dest = new BindableReactiveProperty<string>(pair.Dest);
        }

        internal void ApplyChanges()
        {
            this.Parent.Source = this.Source.Value;
            this.Parent.Dest = this.Dest.Value;
        }
    }
}
