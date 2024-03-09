using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigQueryViewGenerator.Models
{
    public class DocumentGenerator
    {
        public CreateSQLParameter Parameter { get; }
        private static string TemplatePath => System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "Template.xlsx"
            );

        public DocumentGenerator(CreateSQLParameter p)
        {
            this.Parameter = p;
        }
        private string GetExcelPath(string directoryName)
        {
            return $"ビュー定義書_{System.IO.Path.Combine(directoryName, this.Parameter.DestView.Value.FullName)}.xlsx";
        }
        private string GetTextPath(string directoryName)
        {
            return $"{System.IO.Path.Combine(directoryName, this.Parameter.DestView.Value.FullName)}.txt";
        }

        public async Task ExportAsync(string directoryName)
        {
            await ExportExcelAsync(GetExcelPath(directoryName));
            await ExportTextAsync(GetTextPath(directoryName));
        }
        public async Task ExportExcelAsync(string path)
        {
           await System.Threading.Tasks.Task.Run(() => ExportExcel(path));
        }
        private void ExportExcel(string path)
        {
            System.IO.File.Copy(TemplatePath, path);
            var book = new XLWorkbook(path);

            book.Save();
        }
        public async Task ExportTextAsync(string path)
        {
            await System.IO.File.WriteAllTextAsync(path, this.Parameter.SQLText.Value);
        }
    }
}
