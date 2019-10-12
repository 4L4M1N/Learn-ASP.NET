using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace Learn_ASP.NET.Extentions
{
    public class Extentions
    {
        // For Excel convert
        public static DataTable ConvertCsVtoDataTable(string FilePath)
        {
            DataTable dt = new DataTable();
            StreamReader streamReader = new StreamReader(FilePath);
            string[] headers = streamReader.ReadLine().Split(',');
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }

            while (!streamReader.EndOfStream)
            {
                string[] rows = streamReader.ReadLine().Split(',');
                if (rows.Length > 1)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dataRow[i] = rows[i].Trim();
                    }

                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        // For Excel convert
        public static DataTable CovertXslXtoDataTable(string FilePath, string connectionString)
        {
            OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
            DataTable dataTable = new DataTable();
            try
            {
                oleDbConnection.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT *FROM [SHEET1$]", oleDbConnection);
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
                oleDbDataAdapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                oleDbDataAdapter.Fill(dataSet);
                dataTable = dataSet.Tables[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                oleDbConnection.Close();
            }

            return dataTable;
        }
    }
}