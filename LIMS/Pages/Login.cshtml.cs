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
            isLoginAttempt = false;
            if (username != null && password != null)
            {
                isValid = true;
            }

            if (isValid)
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
                        // Salt the password, convert it to a byte[], then compute the hash of the resulting byte[]
                        var computedHash = hash.ComputeHash(Encoding.ASCII.GetBytes(password + salt));
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