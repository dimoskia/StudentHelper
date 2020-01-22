using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentHelper.Models.DTOs
{
    public class StaffFilter
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public string OrderBy { get; set; } = "FirstName";
        public bool Ascending { get; set; } = true;
    }
}