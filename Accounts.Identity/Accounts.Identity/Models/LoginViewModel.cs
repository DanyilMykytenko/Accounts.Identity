using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Identity.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)] //using to hide the password field
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
