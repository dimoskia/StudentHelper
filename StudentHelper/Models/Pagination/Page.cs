using System.Collections.Generic;

namespace StudentHelper.Models
{
    public class Page<T>
    {

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        public IEnumerable<T> Results { get; set; }

        // The URL to the next page - if null, there are no more pages. 
        public string NextPageUrl { get; set; }
    }
}