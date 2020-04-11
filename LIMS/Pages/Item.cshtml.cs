﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
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

        public class ItemForm
        {
            public string Action { get; private set; }
        }

        public JsonResult OnPost([FromBody]ItemForm form)
        {
            var success = false;
            var isUnavailable = false;
            Console.WriteLine("Item Form Action: " + form.Action);
            if (form.Action == "reserve")
            {
                try
                {
                    var handler = new ConnectionHandler();
                    using (MySqlConnection connection = handler.Connection)
                    {
                        MySqlTransaction trans = connection.BeginTransaction();
                        string sql = "SELECT * FROM BookDetails WHERE ISBN=@ISBN WHERE availablity='available'";
                        
                        MySqlCommand cmd = new MySqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@ISBN", ISBN);
                        cmd.Transaction = trans;
                        MySqlDataReader rdr = cmd.ExecuteReader();

                        var bookId = (string)rdr[0];
                        if (bookId != null)
                        {
                            cmd.CommandText = "UPDATE BookDetails SET availablity='reserved' WHERE bookId=@BOOKID";
                            cmd.Parameters.AddWithValue("@BOOKID", bookId);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "INSERT INTO Reservations(userId, bookId, dateReserved) VALUES (@USERID, @BOOKID, @DATERESERVED)";
                            cmd.Parameters.AddWithValue("@USERID", HttpContext.Session.GetString("userId"));
                            cmd.Parameters.AddWithValue("@BOOKID", bookId);
                            cmd.Parameters.AddWithValue("@DATERESERVED", new DateTime());
                            cmd.ExecuteNonQuery();
                            trans.Commit();
                            success = true;
                        }
                        else
                        {
                            isUnavailable = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine(ex);
                }
            }
            return new JsonResult($"{{\"success\":\"{success}\"," +
                                $"\"isUnavailable\":\"{isUnavailable}\"," +
                                $"\"action\":\"{form.Action}\"}}", new System.Text.Json.JsonSerializerOptions());
        }


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