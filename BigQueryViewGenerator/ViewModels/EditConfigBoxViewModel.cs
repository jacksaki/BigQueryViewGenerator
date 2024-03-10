using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BigQueryViewGenerator.ViewModels
{
    public class EditConfigBoxViewModel : MenuViewModelBase
    {
        public EditConfigBoxViewModel(MainWindowViewModel parent):base(parent)
        {
            this.ViewConfigViewModel = new ViewConfigViewModel(this.Parent.Config.ViewConfig);
            this.TestConfigViewModel = new TestConfigViewModel(this.Parent.Config.TestConfig);

            this.SaveCommand = new ReactiveCommand<Unit>();
            this.SaveCommand.Subscribe(_ =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Save();
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            });
        }
        private void Save()
        {
            this.Parent.Config.ViewConfig.SourceTableDatasetCell = this.ViewConfigViewModel.SourceTableDatasetCell.Value;
            this.Parent.Config.ViewConfig.SourceTableNameCell = this.ViewConfigViewModel.SourceTableNameCell.Value;
            this.Parent.Config.ViewConfig.DestViewDatasetCell = this.ViewConfigViewModel.DestViewDatasetCell.Value;
            this.Parent.Config.ViewConfig.DestViewNameCell = this.ViewConfigViewModel.DestViewNameCell.Value;
            this.Parent.Config.ViewConfig.DescriptionCell = this.ViewConfigViewModel.DescriptionCell.Value;
            this.Parent.Config.ViewConfig.NoColumn = this.ViewConfigViewModel.NoFormula.Value;
            this.Parent.Config.ViewConfig.NoColumn = this.ViewConfigViewModel.NoColumn.Value;
            this.Parent.Config.ViewConfig.DetailStartRow = this.ViewConfigViewModel.DetailStartRow.Value;
            this.Parent.Config.ViewConfig.SQLStartColumn = this.ViewConfigViewModel.SQLStartColumn.Value;
            this.Parent.Config.ViewConfig.SQLEndColumn = this.ViewConfigViewModel.SQLEndColumn.Value;
            this.Parent.Config.ViewConfig.ConverterNameColumn = this.ViewConfigViewModel.ConverterNameColumn.Value;
            this.Parent.Config.ViewConfig.TestTargetColumn = this.ViewConfigViewModel.TestTargetColumn.Value;
            this.Parent.Config.ViewConfig.TestTargetFormula = this.ViewConfigViewModel.TestTargetFormula.Value;
            this.Parent.Config.ViewConfig.TestResultColumn = this.ViewConfigViewModel.TestResultColumn.Value;
            this.Parent.Config.ViewConfig.TestResultFormula = this.ViewConfigViewModel.TestResultFormula.Value;
            this.Parent.Config.ViewConfig.SourceColumnNameColumn = this.ViewConfigViewModel.SourceColumnNameColumn.Value;
            this.Parent.Config.ViewConfig.SourceColumnDataTypeColumn = this.ViewConfigViewModel.SourceColumnDataTypeColumn.Value;

            this.Parent.Config.TestConfig.StartRow = this.TestConfigViewModel.StartRow.Value;
            this.Parent.Config.TestConfig.RowSpan = this.TestConfigViewModel.RowSpan.Value;
            this.Parent.Config.TestConfig.SourceFullTableNameCell = this.TestConfigViewModel.SourceFullTableNameCell.Value;
            this.Parent.Config.TestConfig.DestFullViewNameCell = this.TestConfigViewModel.DestFullViewNameCell.Value;
            this.Parent.Config.TestConfig.SQLTitleColumn = this.TestConfigViewModel.SQLTitleColumn.Value;
            this.Parent.Config.TestConfig.SQLColumn = this.TestConfigViewModel.SQLColumn.Value;
            this.Parent.Config.TestConfig.SourceColumnTitleColumn = this.TestConfigViewModel.SourceColumnTitleColumn.Value;
            this.Parent.Config.TestConfig.SourceColumnNameColumn = this.TestConfigViewModel.SourceColumnNameColumn.Value;
            this.Parent.Config.TestConfig.ResultTitleColumn = this.TestConfigViewModel.ResultTitleColumn.Value;
            this.Parent.Config.TestConfig.ResultColumn = this.TestConfigViewModel.ResultColumn.Value;
        }

        public ViewConfigViewModel ViewConfigViewModel { get; }
        public TestConfigViewModel TestConfigViewModel { get; }
        public ReactiveCommand<Unit> SaveCommand { get; }
    }
}
