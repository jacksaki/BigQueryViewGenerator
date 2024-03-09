using BigQueryViewGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.ViewModels
{
    public class MenuViewModelBase:ViewModelBase
    {
        public MenuViewModelBase(MainWindowViewModel parent) : base()
        {
            this.Parent = parent;
        }
        public MainWindowViewModel Parent { get; }
        public AppConfig Config => this.Parent.Config;
    }
}
