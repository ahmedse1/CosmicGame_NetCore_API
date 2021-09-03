using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Request
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string URL { get; set; }

    }
}
