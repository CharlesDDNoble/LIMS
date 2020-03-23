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
        public void OnPost(string username, string password)
        {
            if (username == null && password == null)
            {
                
            }
        }
    }
}