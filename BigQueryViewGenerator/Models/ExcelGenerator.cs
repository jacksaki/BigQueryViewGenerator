using ClosedXML.Excel;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Preview;

namespace BigQueryViewGenerator.Models
{
    public class ExcelGenerator(CreateSQLParameter p)
    {
        private static readonly string ROW_TEXT = "{ROW}";
        public CreateSQLParameter Parameter => p;
        private static string TemplatePath => System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "Template.xlsx"
            );

        public async Task ExportAsync(string path)
        {
            await System.Threading.Tasks.Task.Run(() => Export(path));
        }

        private void Export(string path)
        {
            System.IO.File.Copy(TemplatePath, path);
            var book = new XLWorkbook(path);
            DrawViewSheet(book.Worksheet("View"));
            DrawTestSheet(book.Worksheet("Test"));
            book.Save();
        }

        private void DrawViewSheet(IXLWorksheet sheet)
        {
            var config = this.Parameter.Config.ViewConfig;
            sheet.Cell(config.DescriptionCell).SetValue(XLCellValue.FromObject(this.Parameter.Description));
            sheet.Cell(config.DestViewDatasetCell).SetValue(XLCellValue.FromObject(this.Parameter.DestView.Value.SchemaName));
            sheet.Cell(config.DestViewNameCell).SetValue(XLCellValue.FromObject(this.Parameter.DestView.Value.TableName));
            sheet.Cell(config.SourceTableDatasetCell).SetValue(XLCellValue.FromObject(this.Parameter.SourceTable.Value.SchemaName));
            sheet.Cell(config.SourceTableNameCell).SetValue(XLCellValue.FromObject(this.Parameter.SourceTable.Value.TableName));
            for (var i = 0; i < this.Parameter.Columns.Count; i++)
            {
                var column = this.Parameter.Columns[i];
                var row = config.DetailStartRow + i;
                sheet.Cell($"{config.ConverterNameColumn}{row}").SetValue(XLCellValue.FromObject(column.Converter.Value.Name));
                sheet.Cell($"{config.NoColumn}{row}").SetFormulaA1(config.NoFormula.Replace(ROW_TEXT,row.ToString()));
                sheet.Cell($"{config.SourceColumnDataTypeColumn}{row}").SetValue(XLCellValue.FromObject(column.DataType));
                sheet.Cell($"{config.SourceColumnNameColumn}{row}").SetValue(XLCellValue.FromObject(column.Name));
                sheet.Cell($"{config.SQLStartColumn}{row}").SetValue(XLCellValue.FromObject(column.CreateSQL()));
                sheet.Cell($"{config.TestResultColumn}{row}").SetFormulaA1(config.TestResultFormula.Replace(ROW_TEXT,row.ToString()));
                sheet.Cell($"{config.TestTargetColumn}{row}").SetFormulaA1(config.TestTargetFormula.Replace(ROW_TEXT,row.ToString()));
            }
        }
        private void DrawTestSheet(IXLWorksheet sheet)
        {
            var config = this.Parameter.Config.TestConfig;

            sheet.Cell(config.DestFullViewNameCell).SetValue(XLCellValue.FromObject(this.Parameter.DestView.Value.FullName));
            sheet.Cell(config.SourceFullTableNameCell).SetValue(XLCellValue.FromObject(this.Parameter.SourceTable.Value.FullName));

            var columns = this.Parameter.Columns.Where(x => x.IsExportTarget.Value && x.Converter.Value.IsTestTarget).ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var row = config.StartRow + (i * config.RowSpan);
                sheet.Cell($"{config.SQLColumn}{row}").SetValue(XLCellValue.FromObject(config.SQLTitleColumn));
                sheet.Cell($"{config.SourceColumnNameColumn}{row}").SetValue(XLCellValue.FromObject(config.SourceColumnTitleColumn));
                sheet.Cell($"{config.ResultColumn}{row}").SetValue(XLCellValue.FromObject(config.ResultTitleColumn));

                sheet.Cell($"{config.SQLColumn}{row + 1}").SetValue(XLCellValue.FromObject(column.CreateSQL()));
                sheet.Cell($"{config.SourceColumnNameColumn}{row + 1}").SetValue(XLCellValue.FromObject(column.Name));
            }
        }

    }
}
