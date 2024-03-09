using BigQueryViewGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        public MainWindowViewModel(AppConfig config):base()
        {
            this.Config = config;
            this.EditDatasetBoxViewModel = new EditDatasetBoxViewModel(this);
            this.EditConfigBoxViewModel = new EditConfigBoxViewModel(this);
            this.EditConverterBoxViewModel = new EditConverterBoxViewModel(this);
            this.EditDatasetBoxViewModel = new EditDatasetBoxViewModel(this);
            this.HomeBoxViewModel = new HomeBoxViewModel(this);
            this.ThemeBoxViewModel = new ThemeBoxViewModel(this);
        }
        public AppConfig Config { get; }
        public EditConfigBoxViewModel EditConfigBoxViewModel { get; }
        public EditConverterBoxViewModel EditConverterBoxViewModel { get; }
        public EditDatasetBoxViewModel EditDatasetBoxViewModel { get; }
        public HomeBoxViewModel HomeBoxViewModel { get; }
        public ThemeBoxViewModel ThemeBoxViewModel { get; }
    }
}
 