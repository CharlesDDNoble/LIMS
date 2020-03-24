using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace LIMS
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {

        }

        public class RegistrationForm
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string Username { get; set; }
            public string Password1 { get; set; }
            public string Password2 { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Phone { get; set; }
            public string Zip { get; set; }
        }


        public JsonResult OnPost([FromBody]RegistrationForm form)
        {
            try
            {
                var handler = new ConnectionHandler();
                MySqlTransaction trans = handler.Connection.BeginTransaction();
                string sql = "SELECT * FROM Users WHERE username=@USERNAME";
                MySqlCommand cmd = new MySqlCommand(sql, trans.Connection);
                cmd.Parameters.AddWithValue("@USERNAME", form.Username);
                MySqlDataReader rdr = cmd.ExecuteReader();

                var hasNull = false;
                var hasDifPass = false;
                var isUnameTaken = false;
                var hasError = true;

                if (form.firstName == null || form.lastName == null || form.Username == null || form.Password1 == null || form.Password2 == null
                    || form.Address == null || form.City == null || form.State == null || form.Zip == null || form.Phone == null)
                {
                    hasNull = true;
                }

                if (form.Password1 != form.Password2 && !hasError)
                {
                    hasDifPass = true;
                }

                if (!rdr.Read() && !hasError)
                {
                    Console.WriteLine("Register: No Error in fields");
                    // create a random salt to hash the password with to guard against rainbow tables and other site breaches
                    var rand = new Random((int)Stopwatch.GetTimestamp());
                    SHA256 hash = SHA256.Create();

                    var salt = hash.ComputeHash(Encoding.ASCII.GetBytes(form.Username + rand.Next().ToString()));
                    var hashedPassword = hash.ComputeHash(Encoding.ASCII.GetBytes(form.Password1 + salt));

                    sql = "INSERT INTO Users(username,password,salt,accountType,firstName,lastName,address,city,zip,state,phone) " +
                          "VALUES (@USERNAME, @PASSWORD, @SALT, 'guest', @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @STATE, @PHONE)";
                    cmd = new MySqlCommand(sql, trans.Connection);
                    cmd.Parameters.AddWithValue("@USERNAME", form.Username);
                    cmd.Parameters.AddWithValue("@PASSWORD", hashedPassword);
                    cmd.Parameters.AddWithValue("@SALT", salt);
                    cmd.Parameters.AddWithValue("@FIRSTNAME", form.firstName);
                    cmd.Parameters.AddWithValue("@LASTNAME", form.lastName);
                    cmd.Parameters.AddWithValue("@ADDRESS", form.Address);
                    cmd.Parameters.AddWithValue("@CITY", form.City);
                    cmd.Parameters.AddWithValue("@ZIP", form.Zip);
                    cmd.Parameters.AddWithValue("@STATE", form.State);
                    cmd.Parameters.AddWithValue("@PHONE", form.Phone);
                    rdr = cmd.ExecuteReader();
                    trans.Commit();
                }
                else
                {
                    isUnameTaken = true;
                }
                rdr.Close();
                if (hasError)
                {
                    return new JsonResult($"{{\"hasNullField\":\"{hasNull}\"," +
                                          $"\"hasDifferentPasswords\":\"{hasDifPass}\"," +
                                          $"\"isUsernameTaken\":\"{isUnameTaken}\"," +
                                          $"\"success\":\"false\"}}", new System.Text.Json.JsonSerializerOptions());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new JsonResult("{\"success\":\"true\"}", new System.Text.Json.JsonSerializerOptions());
        }
    }
}