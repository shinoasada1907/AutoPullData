using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoData
{
    public class Model
    {
        public string fileName { get; set; } = "";
        public string leader { get; set; } = "";
        public DateTime startTime { get; set; } = DateTime.MinValue;
        public DateTime completeTime { get; set; } = DateTime.MinValue;
        public string amMachine { get; set; } = "";
        public string shift { get; set; } = "";
        public string lineName { get; set; } = "";
        public string productId { get; set; } = "";
        public string status { get; set; } = "";
        public string result { get; set; } = "";
        public string puncherMachine { get; set; } = "";
        public string heatPuncher { get; set; } = "";
    }

    public class ColumnName
    {
        static string fileName = Function.RemoveDiacritics("File Name");
        static string employeeID = Function.RemoveDiacritics("Mã số nhân viên");
        static string starttime = Function.RemoveDiacritics("Start time");
        static string completetime = Function.RemoveDiacritics("Completion time");
        static string am_machine = Function.RemoveDiacritics("Chọn máy thực hiện AM");
        static string shift = Function.RemoveDiacritics("Ca thực hiện");
        static string cell = Function.RemoveDiacritics("Chọn chuyền");
        static string product = Function.RemoveDiacritics("Chọn tên sản phẩm");
        static string status = Function.RemoveDiacritics("Tổng kết: trong quá trình AM có bất thường gì không?");
        static string result = Function.RemoveDiacritics("Tổng kết: chọn bất thường trong quá trình làm AM, có thể chọn nhiều trường hợp");
        static string puncher = Function.RemoveDiacritics("Chọn máy dập nhiệt thực hiện AM - nếu là máy dập thường chọn N/A");
        static string heat = Function.RemoveDiacritics("Chọn máy dập thường thực hiện AM - nếu là máy dập nhiệt chọn N/A");

        public static string GetFileName()
        {
            return fileName;
        }
    }
}
