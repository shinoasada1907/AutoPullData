using AutoData;
using OfficeOpenXml;
using System.Diagnostics;

var rootFolderPath = "C:\\Users\\sesa744913\\OneDrive - Schneider Electric\\AM Master Data";
var outputFilePath = "C:\\Project\\Data Excel\\AM Data\\AM_DATA.xlsx";
DataAccessLayer data = new DataAccessLayer();
// Define the column names to extract
var columnNames = new string[]
{
    "File Name",
    "Mã số nhân viên",
    "Start time",
    "Completion time",
    "Chọn máy thực hiện AM",
    "Ca thực hiện",
    "Chọn chuyền",
    "Chọn tên sản phẩm",
    "Tổng kết: trong quá trình AM có bất thường gì không?",
    "Tổng kết: chọn bất thường trong quá trình làm AM, có thể chọn nhiều trường hợp",
    "Chọn máy dập nhiệt thực hiện AM",
    "Chọn máy dập thường thực hiện AM"
};

//foreach (var column in columnNames)
//{
//    Console.WriteLine(Function.RemoveDiacritics(column));
//}

Stopwatch stopwatch = new Stopwatch();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//using (var outputPackage = new ExcelPackage(new FileInfo(outputFilePath)))
//{
//    var outputWorksheet = outputPackage.Workbook.Worksheets.Add("Form1");

//    // Write the column names to the output file
//    for (int i = 0; i < columnNames.Length; i++)
//    {
//        outputWorksheet.Cells[1, i + 1].Value = columnNames[i];
//    }
//    List<Model> models = new List<Model>();

//    // Process each Excel file in the directory and its subdirectories
//    Function.ProcessDirectory(rootFolderPath, columnNames, models);

//    Console.WriteLine(models.Count);
//    outputWorksheet.Cells["A1"].LoadFromCollection(models, PrintHeaders: true);
//    outputPackage.Save();

//    //Function.AddData(models);
//}


List<Model> models = new List<Model>();
stopwatch.Start();
// Process each Excel file in the directory and its subdirectories
Function.ProcessDirectory(rootFolderPath, columnNames, models);
//data.AddData(models);
//data.AddDataWithBulk(models);

data.AddDataWithBulkMultiThread(models);
data.AddDataToRootTable();

stopwatch.Stop();
TimeSpan ts = stopwatch.Elapsed;
string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
Console.WriteLine("Time to Pull Data: " + elapsedTime);



