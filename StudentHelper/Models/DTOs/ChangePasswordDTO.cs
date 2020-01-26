using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentHelper.Models.DTOs
{
    public class ChangePasswordDTO
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }

    }
}