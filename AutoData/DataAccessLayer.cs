using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace AutoData
{
    public class DataAccessLayer
    {
        private const string connectionString = "Data Source=10.155.128.23;Initial Catalog=SEMV_AM;Persist Security Info=True;User ID=semv;Password=Semv@123;MultipleActiveResultSets=true;TrustServerCertificate=True;";

        public void AddDataWithBulk(List<Model> models)
        {
            DateTime targetDate = new DateTime(2024, 1, 10);
            models = models.Where(model => model.startTime >= targetDate).ToList();
            DataTable table = new DataTable();
            table.Columns.Add("fileName");
            table.Columns.Add("leader");
            table.Columns.Add("startTime");
            table.Columns.Add("completeTime");
            table.Columns.Add("amMachine");
            table.Columns.Add("shift");
            table.Columns.Add("lineName");
            table.Columns.Add("productId");
            table.Columns.Add("status");
            table.Columns.Add("result");
            table.Columns.Add("puncherMachine");
            table.Columns.Add("heatPuncher");

            foreach (Model model in models)
            {
                Console.WriteLine(models.IndexOf(model));
                table.Rows.Add(model.fileName, model.leader, model.startTime, model.completeTime, model.amMachine, Convert.ToInt32(model.shift.Trim()), model.lineName, model.productId, model.status, model.result, model.puncherMachine, model.heatPuncher);
            }

            string query = "truncate table [SEMV_AM].[dbo].[myModels]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlBulkCopy bulk = new SqlBulkCopy(connection))
                {
                    bulk.DestinationTableName = "[SEMV_AM].[dbo].[myModels]";
                    bulk.WriteToServer(table);
                }
                connection.Close();
            }

            Console.WriteLine("Pull Data Successfull!!!");
        }

        public void AddDataWithBulkMultiThread(List<Model> models)
        {
            DateTime targetDate = new DateTime(2024, 1, 10);
            models = models.Where(model => model.startTime >= targetDate).ToList();

            string query = "truncate table [SEMV_AM].[dbo].[myModels]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

            List<List<Model>> subList = new List<List<Model>>();
            int batchSize = 10000;
            for (int i = 0; i < models.Count; i += batchSize)
            {
                var batch = models.Skip(i).Take(batchSize).ToList();
                Console.WriteLine(i);
                foreach (var model in batch)
                {
                    model.shift = model.shift.Trim();
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlBulkCopy bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulk.DestinationTableName = "[SEMV_AM].[dbo].[myModels]";
                            bulk.BulkCopyTimeout = 3600;
                            DataTable table = ConvertToDataTable(batch);
                            bulk.WriteToServer(table);
                        }
                        transaction.Commit();
                    }

                    connection.Close();
                }
            }

            Console.WriteLine("Pull Data Successfull!!!");
        }

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Lấy tất cả các thuộc tính
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                // Đặt tên cột là tên của các thuộc tính
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    // chèn giá trị của thuộc tính vào các hàng của datatable
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }


        public void AddDataToRootTable()
        {
            try
            {
                string query = "[dbo].[INSERT_DATA_AM_DATA]";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Add Data to Root Table Success!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception AddDataToRootTable: " + ex.Message);
            }
        }
    }
}
