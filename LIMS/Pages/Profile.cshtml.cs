using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;

namespace LIMS
{
    public class ProfileModel : PageModel
    {


        public class ProfileForm
        {
            public string Action { get; set; }
            public string Data { get; set; }
        }


        public JsonResult handleRequest(ProfileForm form)
        {
            var success = false;
            try
            {
                var handler = new ConnectionHandler();
                using (MySqlConnection connection = handler.Connection)
                {
                    string sql = "SELECT requestId FROM BookRequests WHERE ISBN=@ISBN AND userId=@USERID";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@ISBN", form.Data);
                    cmd.Parameters.AddWithValue("@USERID", HttpContext.Session.GetString("userId"));
                    int requestId = -1;

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            requestId = (int)rdr[0];
                        }
                    }

                    // if there is already a request of this book from the user, ignore this
                    if (requestId != -1)
                    {
                        success = true;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO BookRequests(userId, ISBN, dateRequested) VALUES (@USERID, @ISBN, CURDATE())";
                        cmd.ExecuteNonQuery();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                Console.WriteLine(ex);
            }

            var reason = "unknown";
            if (success)
            {
                reason = "none";
            }
            return new JsonResult($"{{\"success\":\"{success}\"," +
                                    $"\"reason\":\"{reason}\"}}", new System.Text.Json.JsonSerializerOptions());
        }

        public JsonResult OnPost([FromBody]ProfileForm form)
        {
            JsonResult res = new JsonResult("{\"success\":\"False\"," +
                                            "\"reason\":\"none\"}", new System.Text.Json.JsonSerializerOptions());
            if (form.Action == "request")
            {
                res = handleRequest(form);
            }

            return res;
        }
        
        public void OnGet()
        {

        }
    }
}