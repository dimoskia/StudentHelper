using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StudentHelper.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual UserDetails UserDetails { get; set; }
        [JsonIgnore]
        public int UserDetailsId { get; set; }
        [JsonIgnore]
        public virtual Course Course { get; set; }
        [JsonIgnore]
        public int CourseId { get; set; }
    }
}