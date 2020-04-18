using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


namespace LIMS
{
    public class ManagerModel : PageModel
    {
        public List<BookRequest> BookRequestList { get; private set; } = new List<BookRequest>();
        public List<Order> OrderList { get; private set; } = new List<Order>();

        public class BookRequest
        {
            public BookRequest(string isbn, long userCount)
            {
                this.ISBN = isbn;
                this.UserCount = userCount;
            }
            public string ISBN { get; set; }
            public long UserCount { get; set; }
        }

        public class Order
        {
            public Order(int orderId, string isbn, int quantity,
                DateTime dateOrdered, DateTime dateExpected, DateTime dateRecieved)
            {
                this.OrderId = orderId;
                this.ISBN = isbn;
                this.Quantity = quantity;
                this.DateOrdered = dateOrdered;
                this.DateExpected = dateExpected;
                this.DateRecieved = dateRecieved;
            }
            public int OrderId { get; set; }
            public string ISBN { get; set; }
            public int Quantity { get; set; }
            public DateTime DateOrdered { get; set; }
            public DateTime DateExpected { get; set; }
            public DateTime DateRecieved { get; set; }
        }


        public void FetchAllBookRequests()
        {
            BookRequestList = new List<BookRequest>();
            try
            {
                var handler = new ConnectionHandler();
                using (MySqlConnection connection = handler.Connection)
                {
                    string sql = "SELECT ISBN, COUNT(userId) as userCount FROM lims.BookRequests GROUP BY ISBN ORDER BY userCount DESC;";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string isbn = rdr[0].ToString();
                            long userCount = (long)rdr[1];
                            BookRequest request = new BookRequest(isbn, userCount);
                            BookRequestList.Add(request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void FetchAllOrders()
        {
            OrderList = new List<Order>();
            try
            {
                var handler = new ConnectionHandler();
                using (MySqlConnection connection = handler.Connection)
                {
                    string sql = "SELECT * FROM Orders";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int bookId = (int)rdr[0];
                            string isbn = rdr[1].ToString();
                            int quantity = (int)rdr[2];
                            DateTime dateOrdered = (DateTime)rdr[3];
                            DateTime dateExpected = (DateTime)rdr[4];
                            DateTime dateRecieved = (DateTime)rdr[5];
                            Order order = new Order(bookId, isbn, quantity, dateOrdered, dateExpected, dateRecieved);
                            OrderList.Add(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void OnGet()
        {
            FetchAllBookRequests();
            FetchAllOrders();
        }
    }
}