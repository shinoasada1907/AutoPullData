using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidecode.NET;

namespace AutoData
{
    public class Function
    {
        static int count = 0;
        public static string RemoveDiacritics(string text)
        {
            //var normalizedString = text.Normalize(NormalizationForm.FormD);
            //var stringBuilder = new StringBuilder();

            //foreach (var c in normalizedString)
            //{
            //    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            //    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            //    {
            //        stringBuilder.Append(c);
            //    }
            //}
            //string output = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            string textInput = text.Unidecode().ToLower();

            return textInput.Replace(" ", "");
        }

        //public static void AddData (List<Model> models)
        //{
        //    using (var db = new MyDbContext())
        //    {
        //        db.MyTableAM.AddRange(models);
        //        db.SaveChanges();
        //    }
        //}

        public static void ProcessDirectory(string directoryPath, string[] columnNames, List<Model> listModel)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(directoryPath);
            foreach (string filePath in fileEntries)
            {
                //Console.WriteLine(filePath);
                if (Path.GetExtension(filePath) == ".xlsx" || Path.GetExtension(filePath) == ".xls")
                {
                    //Console.WriteLine(filePath);
                    ProcessExcelFile(filePath, columnNames, listModel);
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(directoryPath);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, columnNames, listModel);
        }

        static void ProcessExcelFile(string filePath, string[] columnNames, List<Model> listModel)
        {
            ColumnName name = new ColumnName();
            using (var inputPackage = new ExcelPackage(new FileInfo(filePath)))
            {
                var inputWorksheet = inputPackage.Workbook.Worksheets["Form1"]; // assuming you want the first worksheet

                // Find the indices of the columns to extract
                var columnIndices = columnNames.Select(name =>
                {
                    if (inputWorksheet.Dimension != null)
                    {
                        for (int col = 1; col <= inputWorksheet.Dimension.Columns; col++)
                        {
                            if (!string.IsNullOrEmpty(RemoveDiacritics(inputWorksheet.Cells[1, col].Value as string)) && RemoveDiacritics(inputWorksheet.Cells[1, col].Value as string) == RemoveDiacritics(name))
                            {
                                return col;
                            }
                        }
                    }
                    return -1; // column not found
                }).ToArray();

                Console.WriteLine(Path.GetFileName(filePath) + "=> Column: " + columnIndices.Length + ": Row: " + inputWorksheet.Dimension.Rows);
                //Console.WriteLine(Path.GetFileName(filePath) + " => " + columnIndices[10]);
                //Console.WriteLine(Path.GetFileName(filePath) + " => " + columnIndices[11]);

                // Copy the data from the input file to the output file
                for (int row = 2; row <= inputWorksheet.Dimension.Rows; row++)
                {
                    Model model = new Model();
                    for (int i = 0; i < columnIndices.Length; i++)
                    {
                        if (columnIndices[i] != -1)
                        {
                            //Console.WriteLine(Path.GetFileName(filePath) + " => " + i + " => " + columnIndices[i] + " => " + columnNames[i] + " => " + RemoveDiacritics(columnNames[i]));
                            model.fileName = Path.GetFileName(filePath);
                            switch (RemoveDiacritics(columnNames[i]))
                            {
                                case "masonhanvien":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        model.leader = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "starttime":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        double sttime = Convert.ToDouble(inputWorksheet.Cells[row, columnIndices[i]].Value.ToString());
                                        model.startTime = DateTime.FromOADate(sttime);
                                    }
                                    break;
                                case "completiontime":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        double ctime = Convert.ToDouble(inputWorksheet.Cells[row, columnIndices[i]].Value.ToString());
                                        model.completeTime = DateTime.FromOADate(ctime);
                                    }
                                    break;
                                case "chonmaythuchienam":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        model.amMachine = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "cathuchien":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        model.shift = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "chonchuyen":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        model.lineName = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "chontensanpham":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        model.productId = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "tongket:trongquatrinhamcobatthuonggikhong?":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        model.status = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "tongket:chonbatthuongtrongquatrinhlamam,cothechonnhieutruonghop":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        model.result = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "chonmaydapnhietthuchienam":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        Console.WriteLine(Path.GetFileName(filePath) + ": Index = " + i + " => " + columnIndices[i]);
                                        model.puncherMachine = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                case "chonmaydapthuongthuchienam":
                                    if (inputWorksheet.Cells[row, columnIndices[i]] != null && inputWorksheet.Cells[row, columnIndices[i]].Value != null)
                                    {
                                        //Console.WriteLine(columnNames[i] + ": " + inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "");
                                        //Console.WriteLine(Path.GetFileName(filePath) + ": Index = " + i + " => " + columnIndices[i]);
                                        model.heatPuncher = inputWorksheet.Cells[row, columnIndices[i]].Value.ToString() ?? "";
                                    }
                                    break;
                                default:
                                    break;
                            }
                            //outputWorksheet.Cells[row, 1].Value = Path.GetFileName(filePath);
                            //outputWorksheet.Cells[row, i + 1].Value = inputWorksheet.Cells[row, columnIndices[i]].Value;
                        }
                    }
                    listModel.Add(model);
                }
            }
        }
    }
}
