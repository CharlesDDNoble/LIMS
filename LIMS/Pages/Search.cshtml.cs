using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LIMS
{
    public class SearchModel : PageModel
    {
        public string Filter { get; private set; }
        public string Search { get; private set; }
        public List<Dictionary<string, string>> Results { get; private set; } = new List<Dictionary<string, string>>();

        public void OnGet(string filter, string search)
        {

            if (filter == "Title" || filter == "Author" || filter == "Search")
            {
                Filter = filter;
            } else
            {
                Filter = "Title";
            }

            if (search == null)
            {
                Search = "";
            }
            Search = "%" + Search + "%";

            var handler = new ConnectionHandler();
            var connection = handler.Connection;

            try
            {
                string sql = "SELECT imagePath, title, author, genre, summary, ISBN FROM Books WHERE @Filter LIKE @Search";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Filter", Filter);
                cmd.Parameters.AddWithValue("@Search", Search);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string summary = (string)rdr[4];
                    if (summary.Length >= 150)
                    {
                        summary = summary.Remove(150, summary.Length-1-150) + "...";
                    }

                    Dictionary<string, string> row = new Dictionary<string, string>
                    {
                        { "imagePath", (string)rdr[0] },
                        { "title", (string)rdr[1] },
                        { "author", (string)rdr[2] },
                        { "genre", (string)rdr[3] },
                        { "summary", summary },
                        { "ISBN", (string)rdr[5] }
                    };
                    Results.Add(row);
                    //Console.WriteLine(String.Format("[imagePath:{0},title:{1},author:{2},genre:{3},summary:{4},ISBN:{5}]",
                                                    //row["imagePath"], row["title"], row["author"], row["genre"], row["summary"], row["ISBN"]));
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            connection.Close();
        }
    }
}