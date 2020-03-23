using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LIMS
{
    public class ItemModel : PageModel
    {
        public string Title { get; private set; } = "Sample Title of a Book";
        public string Author { get; private set; } = "John Doe";
        public string Genre { get; private set; } = "Fantasy";
        public DateTime DatePublished { get; private set; }
        public string ISBN { get; private set; } = "0000-0000-00000";
        public string Summary { get; private set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        public string ImagePath { get; private set; } = "placeholder-2.png";
        public string Log { get; private set; } = "";

        public void OnGet()
        {
            var handler = new ConnectionHandler();
            var connection = handler.Connection;
            try
            {
                Log += "Connecting to MySQL...<br>";

                string sql = "SELECT * FROM Books WHERE ISBN='0000000000000'";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                rdr.Read();

                this.ISBN = (string) rdr[0];
                this.Title = (string) rdr[1];
                this.Genre = (string) rdr[2];
                this.Author = (string) rdr[3];
                this.Summary = (string) rdr[4];
                this.DatePublished = (DateTime) rdr[5];
                this.ImagePath = (string)rdr[6];

                rdr.Close();
            }
            catch (Exception ex)
            {
                Log += ex.ToString()+ "<br>";
            }

            connection.Close();
            Log += "Done.<br>";
        }
    }
}