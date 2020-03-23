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
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Genre { get; private set; }
        public DateTime DatePublished { get; private set; }
        public string ISBN { get; private set; }
        public string Summary { get; private set; } 
        public string ImagePath { get; private set; }
        public string Log { get; private set; }

        public void OnGet(string isbn)
        {

            if (isbn == null)
            {
                ISBN = "0000000000000";
            } else
            {
                ISBN = isbn;
            }

            try
            {
                var handler = new ConnectionHandler();
                MySqlConnection connection = handler.Connection;
                string sql = "SELECT * FROM Books WHERE ISBN=@ISBN";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ISBN", ISBN);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    //this.ISBN = (string) rdr[0];
                    Title = (string)rdr[1];
                    Genre = (string)rdr[2];
                    Author = (string)rdr[3];
                    Summary = (string)rdr[4];
                    DatePublished = (DateTime)rdr[5];
                    ImagePath = (string)rdr[6];
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