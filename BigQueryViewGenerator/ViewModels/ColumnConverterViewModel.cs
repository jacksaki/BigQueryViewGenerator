using BigQueryViewGenerator.Models;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigQueryViewGenerator.ViewModels
{
    public class ColumnConverterViewModel : ViewModelBase
    {
        public ColumnConverter Parent { get; }
        public bool IsManual => this.Parent.IsManual;
        public bool IsReadonly => this.Parent.IsReadonly;
        public bool IsNew { get; private set; }
        public BindableReactiveProperty<string> Name { get; }
        public BindableReactiveProperty<bool> IsTestTarget { get; }
        public BindableReactiveProperty<string> Template { get; }
        public static ColumnConverterViewModel CreateNew(IEnumerable<ColumnConverter> converters)
        {
            var converter = converters.CreateNew();
            return new ColumnConverterViewModel(converter) { IsNew = true };
        }
        public ColumnConverterViewModel(ColumnConverter parent) :base()
        {
            this.Parent = parent;
            this.Name = new BindableReactiveProperty<string>(this.Parent.Name);
            this.IsTestTarget = new BindableReactiveProperty<bool>(this.Parent.IsTestTarget);
            this.Template = new BindableReactiveProperty<string>(this.Parent.Template);
        }
        public void ApplyChanges()
        {
            this.Parent.Name = this.Name.Value;
            this.Parent.Template = this.Template.Value;
            this.Parent.IsTestTarget = this.IsTestTarget.Value;
        }
    }
}
