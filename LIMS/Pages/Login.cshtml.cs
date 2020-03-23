using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LIMS
{
    public class LoginModel : PageModel
    {
        public bool isValid { get; private set; } = false;
        public void OnPost(string username, string password)
        {
            if (username == null && password == null)
            {

            }
        }
    }
}