using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace StudentHelper.Models
{

    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string Program { get; set; }
        public string Semester { get; set; }
        public string Description { get; set; }
        public string DetailsUrl { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Staff> Professors { get; set; }
        public virtual ICollection<Staff> Assistants { get; set; }
        public virtual ICollection<Popularity> PopularityStats { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Members { get; set; }
        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; }
        public static IQueryable<Course> FilterCourses(IQueryable<Course> courses, CourseFilter courseFilter)
        {
            IQueryable<Course> result = courses.Where(c => courseFilter.Year.Contains(c.Year))
                .Where(c => courseFilter.Program.Contains(c.Program))
                .Where(c => courseFilter.Semester.Contains(c.Semester))
                .Where(c => courseFilter.Type.Contains(c.Type));

            if (!string.IsNullOrEmpty(courseFilter.SearchTerm))
            {
                result = result.Where(c => c.Title.ToLower().Contains(courseFilter.SearchTerm.ToLower()));

            }

            return result;
        }
        
    }
}