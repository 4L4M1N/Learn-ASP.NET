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

        ////Password Hash

        //public static string CreateHash(string password, string salt)
        //{
        //    // Get a byte array containing the combined password + salt.
        //    string authDetails = password + salt;
        //    byte[] authBytes = System.Text.Encoding.ASCII.GetBytes(authDetails);

        //    // Use MD5 to compute the hash of the byte array, and return the hash as
        //    // a Base64-encoded string.
        //    var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    byte[] hashedBytes = md5.ComputeHash(authBytes);
        //    string hash = Convert.ToBase64String(hashedBytes);

        //    return hash;
        //}

        //// Check to see if the given password and salt hash to the same value
        //// as the given hash.
        //public static bool IsMatchingHash(string password, string salt, string hash)
        //{
        //    // Recompute the hash from the given auth details, and compare it to
        //    // the hash provided by the cookie.
        //    return CreateHash(password, salt) == hash;
        //}

        //// Create an authentication cookie that stores the username and a hash of
        //// the password and salt.
        //public static HttpCookie CreateAuthCookie(string username, string password, string salt)
        //{
        //    // Create the cookie and set its value to the username and a hash of the
        //    // password and salt. Use a pipe character as a delimiter so we can
        //    // separate these two elements later.
        //    HttpCookie cookie = new HttpCookie("Avalanche");
        //    cookie.Value = username + "|" + CreateHash(password, salt);
        //    return cookie;
        //}

        //// Determine whether the given authentication cookie is valid by
        //// extracting the username, retrieving the saved password, recomputing its
        //// hash, and comparing the hashes to see if they match. If they match,
        //// then this authentication cookie is valid.
        //public static bool IsValidAuthCookie(HttpCookie cookie, string salt)
        //{
        //    // Split the cookie value by the pipe delimiter.
        //    string[] values = cookie.Value.Split('|');
        //    if (values.Length != 2) return false;

        //    // Retrieve the username and hash from the split values.
        //    string username = values[0];
        //    string hash = values[1];

        //    // You'll have to provide your GetPasswordForUser function.
        //    string password = GetPasswordForUser(username);

        //    // Check the password and salt against the hash.
        //    return IsMatchingHash(password, salt, hash);
        //}

        //public static string GetPasswordForUser(string username)
        //{
        //    return "a";
        //}
    }
}