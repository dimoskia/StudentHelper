using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentHelper.Models.DTOs
{
    public class SuccessfulSignInDTO
    {
        public string token { get; set; }
        public User user { get; set; }
    }
}