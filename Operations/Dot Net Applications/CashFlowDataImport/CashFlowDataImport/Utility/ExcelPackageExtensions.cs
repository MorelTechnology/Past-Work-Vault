using OfficeOpenXml;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace CashFlowDataImport
{
    public static class ExcelPackageExtensions
    {
        public static DataTable ToDataTable(this ExcelPackage package, [Optional]string worksheetName)
        {
            ExcelWorksheet workSheet = null;
            if (string.IsNullOrEmpty(worksheetName))
                workSheet = package.Workbook.Worksheets.First();
            else
                workSheet = package.Workbook.Worksheets[worksheetName];

            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }
    }
}
