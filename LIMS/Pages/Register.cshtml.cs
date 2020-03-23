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

namespace LIMS
{
    public class RegisterModel : PageModel
    {
        public bool IsUsernameTaken { get; private set; } = false;
        public bool HasNullField { get; private set; } = false;
        public bool HasDifferentPasswords { get; private set; } = false;
        public bool HasToRollback { get; private set; } = false;

        public void OnPost()
        {

        }
        public IActionResult OnPost(string firstName, string lastName, string username, string password1,
                           string password2, string address, string city, string state, string zip,
                           string phone)
        {
            var redirect = Redirect("/Register");

            try
            {
                var handler = new ConnectionHandler();
                MySqlTransaction trans = handler.Connection.BeginTransaction();
                string sql = "SELECT * FROM Users WHERE username=@USERNAME";
                MySqlCommand cmd = new MySqlCommand(sql, trans.Connection);
                cmd.Parameters.AddWithValue("@USERNAME", username);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (firstName == null || lastName == null || username == null || password1 == null || password2 == null
                    || address == null || city == null || state == null || zip == null || phone == null)
                {
                    HasNullField = true;
                    HasToRollback = true;
                }

                if (password1 != password2 && !HasToRollback)
                {
                    HasDifferentPasswords = true;
                    HasToRollback = true;
                }

                if (!rdr.Read() && !HasToRollback)
                {
                    // create a random salt to hash the password with to guard against rainbow tables and other site breaches
                    var rand = new Random((int)Stopwatch.GetTimestamp());
                    SHA256 hash = SHA256.Create();

                    var salt = hash.ComputeHash(Encoding.ASCII.GetBytes(username + rand.Next().ToString()));
                    var hashedPassword = hash.ComputeHash(Encoding.ASCII.GetBytes(password1 + salt));

                    sql = "INSERT INTO Users(username,password,salt,accountType,firstName,lastName,address,city,zip,state,phone) " +
                          "VALUES (@USERNAME, @PASSWORD, @SALT, 'guest', @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @STATE, @PHONE)";
                    cmd = new MySqlCommand(sql, trans.Connection);
                    cmd.Parameters.AddWithValue("@USERNAME", username);
                    cmd.Parameters.AddWithValue("@PASSWORD", hashedPassword);
                    cmd.Parameters.AddWithValue("@SALT", salt);
                    cmd.Parameters.AddWithValue("@FIRSTNAME", firstName);
                    cmd.Parameters.AddWithValue("@LASTNAME", lastName);
                    cmd.Parameters.AddWithValue("@ADDRESS", address);
                    cmd.Parameters.AddWithValue("@CITY", city);
                    cmd.Parameters.AddWithValue("@ZIP", zip);
                    cmd.Parameters.AddWithValue("@STATE", state);
                    cmd.Parameters.AddWithValue("@PHONE", phone);
                    rdr = cmd.ExecuteReader();
                }
                else
                {
                    IsUsernameTaken = true;
                    HasToRollback = true;
                }
                rdr.Close();

                if (!HasToRollback)
                {
                    trans.Commit();
                    redirect = Redirect("/Login?success=true");
                }
                else
                {
                    redirect = Redirect(String.Format("/Register?error=true&hasnullfield={0}&hasdifferentpasswords={1}&isusernametaken={2}",
                                                                HasNullField, HasDifferentPasswords, IsUsernameTaken));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return redirect;
        }
    }
}