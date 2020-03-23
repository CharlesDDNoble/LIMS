using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Text;

namespace LIMS
{
    public class LoginModel : PageModel
    {
        public bool isLoginAttempt { get; private set; } = false;
        public bool isValid { get; private set; } = false;
        public void OnPost()
        {

        }

        public void OnPost(string username, string password)
        {
            isLoginAttempt = true;
            if (username != null && password != null)
            {
                try
                {
                    var handler = new ConnectionHandler();
                    MySqlConnection connection = handler.Connection;
                    string sql = "SELECT password, salt FROM Users WHERE username=@USERNAME";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@USERNAME", username);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        var hashedPassword = (string)rdr[0];
                        var salt = (string)rdr[1];
                        SHA256 hash = SHA256.Create();
                        // Salt the password, convert it to a byte[], compute the hash, then convert back to string for comparison
                        var computedHash = Encoding.ASCII.GetString(hash.ComputeHash(Encoding.ASCII.GetBytes(password + salt)));
                        
                        if (computedHash.ToString() == hashedPassword)
                        {
                            isValid = true;
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}