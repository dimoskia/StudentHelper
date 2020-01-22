using System.Collections.Generic;

namespace StudentHelper.Models.DTOs
{
    public class EditCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string Program { get; set; }
        public string Semester { get; set; }
        public string DetailsUrl { get; set; }
        public virtual List<int> ProfessorIds { get; set; }
        public virtual List<int> AssistantIds { get; set; }
    }
}