using System.Collections.Generic;

namespace StudentHelper.Models
{
    public class CourseFilter
    {
        public List<int> Year { get; set; } = new List<int> {1, 2, 3, 4};
        public List<string> Program { get; set; } = new List<string> { "КНИ", "ПЕТ", "МТ", "КЕ", "ИКИ", "АСИ", "ПИТ" };
        public List<string> Semester { get; set; } = new List<string> { "зимски", "летен" };
        public List<string> Type { get; set; } = new List<string> { "задолжителен", "изборен" };
        public string SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public bool Favourites { get; set; } = false;

    }
}