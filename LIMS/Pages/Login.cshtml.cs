using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Web;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Text;

namespace LIMS
{
    public class LoginModel : PageModel
    {
        // Is the login information valid?
        public bool IsValid { get; private set; } = false;

        // If success is true, then registration successful
        // else if is false, then it was an unsuccessful login
        public string SuccessString { get; private set; }

        public void OnGet(string success)
        {
            SuccessString = success;
        }

        public IActionResult OnPost(string username, string password)
        {

            var url = "/Login?success=false";

            if (username != null && password != null)
            {
                try
                {
                    var handler = new ConnectionHandler();
                    MySqlConnection connection = handler.Connection;
                    string sql = "SELECT uid, password, salt FROM Users WHERE username=@USERNAME";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@USERNAME", username);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        var uid = (string)rdr[0];
                        var hashedPassword = (string)rdr[1];
                        var salt = (string)rdr[2];
                        SHA256 hash = SHA256.Create();
                        // Salt the password, convert it to a byte[], compute the hash, then convert back to string for comparison
                        var computedHash = Encoding.ASCII.GetString(hash.ComputeHash(Encoding.ASCII.GetBytes(password + salt)));
                        
                        if (computedHash.ToString() == hashedPassword)
                        {
                            HttpContext.Session.Set("userid",Encoding.UTF8.GetBytes(uid));
                            IsValid = true;
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if (IsValid)
            {
               url = "/Index";
            }

            return Redirect(url);
        }
    }
}