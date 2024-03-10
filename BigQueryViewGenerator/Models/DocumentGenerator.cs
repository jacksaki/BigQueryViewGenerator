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
        public DocumentGenerator(CreateSQLParameter p)
        {
            this.Parameter = p;
        }
        private string GetExcelPath(string directoryName)
        {
            return System.IO.Path.Combine(directoryName, $"ビュー定義書_{this.Parameter.DestView.Value.FullName}.xlsx");
        }
        private string GetTextPath(string directoryName)
        {
            return System.IO.Path.Combine(directoryName, $"{this.Parameter.DestView.Value.FullName}.txt");
        }

        public async Task ExportAsync(string directoryName)
        {
            await ExportExcelAsync(GetExcelPath(directoryName));
            await ExportTextAsync(GetTextPath(directoryName));
        }
        public async Task ExportExcelAsync(string path)
        {
            await new ExcelGenerator(this.Parameter).ExportAsync(path);
        }
        public async Task ExportTextAsync(string path)
        {
            await System.IO.File.WriteAllTextAsync(path, this.Parameter.SQLText.Value);
        }
    }
}
