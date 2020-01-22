using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using StudentHelper.Data;

namespace StudentHelper.Models
{
    public class CourseJsonConverter : JsonConverter<Course>
    {

        public StudentHelperContext DbContext { get; set; }

        public override Course ReadJson(JsonReader reader, Type objectType, Course existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            string title = item.GetValue("title").Value<string>();
            string type = item["type"].Value<string>();
            int year = item["year"].Value<int>();
            string program = item["program"].Value<string>();
            string semester = item["semester"].Value<string>();
            string detailsUrl = item["detailsUrl"].Value<string>();

            List<Staff> proffesors = item["professors"]
                .ToObject<IList<int>>()
                .Select(id => DbContext.Staffs.Find(id))
                .Where(staff => staff != null)
                .ToList();

            List<Staff> assistants = item["assistants"]
                .ToObject<IList<int>>()
                .Select(id => DbContext.Staffs.Find(id))
                .Where(staff => staff != null)
                .ToList();

            return new Course()
            {
                Title = title,
                Type = type,
                Year = year,
                Program = program,
                Semester = semester,
                DetailsUrl = detailsUrl,
                Professors = proffesors,
                Assistants = assistants
            };
        }

        public override void WriteJson(JsonWriter writer, Course value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}