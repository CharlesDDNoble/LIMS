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

namespace LIMS
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {

        }

        public JsonResult OnPost(string data)
        {
            //try
            //{
                Console.WriteLine(data);
                //var handler = new ConnectionHandler();
                //MySqlTransaction trans = handler.Connection.BeginTransaction();
                //string sql = "SELECT * FROM Users WHERE username=@USERNAME";
                //MySqlCommand cmd = new MySqlCommand(sql, trans.Connection);
                //cmd.Parameters.AddWithValue("@USERNAME", username);
                //MySqlDataReader rdr = cmd.ExecuteReader();

                //// these bools track the current posts errors, as opposed to the bools that are
                //// past via POST which track the previous request to tell the user what the error was.
                //var hasNull = false;
                //var hasDifPass= false;
                //var isUnameTaken = false;
                //var hasError = true;

            //    if (firstName == null || lastName == null || username == null || password1 == null || password2 == null
            //        || address == null || city == null || state == null || zip == null || phone == null)
            //    {
            //        hasNull = true;
            //    }

            //    if (password1 != password2 && !hasError)
            //    {
            //        hasDifPass = true;
            //    }

            //    if (!rdr.Read() && !hasError)
            //    {
            //        Console.WriteLine("Register: No Error in fields");
            //        // create a random salt to hash the password with to guard against rainbow tables and other site breaches
            //        var rand = new Random((int)Stopwatch.GetTimestamp());
            //        SHA256 hash = SHA256.Create();

            //        var salt = hash.ComputeHash(Encoding.ASCII.GetBytes(username + rand.Next().ToString()));
            //        var hashedPassword = hash.ComputeHash(Encoding.ASCII.GetBytes(password1 + salt));

            //        sql = "INSERT INTO Users(username,password,salt,accountType,firstName,lastName,address,city,zip,state,phone) " +
            //              "VALUES (@USERNAME, @PASSWORD, @SALT, 'guest', @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @STATE, @PHONE)";
            //        cmd = new MySqlCommand(sql, trans.Connection);
            //        cmd.Parameters.AddWithValue("@USERNAME", username);
            //        cmd.Parameters.AddWithValue("@PASSWORD", hashedPassword);
            //        cmd.Parameters.AddWithValue("@SALT", salt);
            //        cmd.Parameters.AddWithValue("@FIRSTNAME", firstName);
            //        cmd.Parameters.AddWithValue("@LASTNAME", lastName);
            //        cmd.Parameters.AddWithValue("@ADDRESS", address);
            //        cmd.Parameters.AddWithValue("@CITY", city);
            //        cmd.Parameters.AddWithValue("@ZIP", zip);
            //        cmd.Parameters.AddWithValue("@STATE", state);
            //        cmd.Parameters.AddWithValue("@PHONE", phone);
            //        rdr = cmd.ExecuteReader();
            //        trans.Commit();
            //    }
            //    else
            //    {
            //        isUnameTaken = true;
            //    }
            //    rdr.Close();
            //    if (hasError) {
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            return new JsonResult("", new System.Text.Json.JsonSerializerOptions());
        }
    }
}