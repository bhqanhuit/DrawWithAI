using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using DrawClientMaui.Services;
using System.Security.Principal;

namespace DrawClientMaui.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
