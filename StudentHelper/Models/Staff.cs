using Newtonsoft.Json;
using System.Collections.Generic;

namespace StudentHelper.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string DetailsUrl { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public virtual ICollection<Course> CoursesProf { get; set; }
        [JsonIgnore]
        public virtual ICollection<Course> CoursesAssist { get; set; }

    }
}