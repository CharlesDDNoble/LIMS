using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LIMS
{
    public class RegisterModel : PageModel
    {
        public void OnPost()
        {

        }
        public void OnPost(string firstName, string lastName, string username, string password1,
                           string password2, string address, string city, string state, string zip,
                           string phone)
        {

        }
    }
}