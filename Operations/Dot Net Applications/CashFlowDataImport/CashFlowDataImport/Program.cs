using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowDataImport
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var importSettings = Configuration.Import.Default;
            DataTable datatable = null; // as temporary DTO

            //// Step 1 -- Get data from excel to a datatable. ////
            using (FileStream stream = File.Open(importSettings.sourceExcelFilePath, FileMode.Open))
                datatable = new ExcelPackage(stream).ToDataTable(importSettings.excelWorksheetName);

            //// Step 2 -- Iterate excel file rows and populate SQL rows  ////
            DateTime importDateStamp = new DateTime(2000, 1, 1);
            foreach (DataRow row in datatable.Rows)
            {
                Utility.Data.importRow(row, importDateStamp);
            }
        }
    }
}