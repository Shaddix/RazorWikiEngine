using System;

namespace RuPM.Models.Database
{
    public class WikiComment
    {
        public int Id { get; set; }
        public virtual WikiPage WikiPage { get; set; }
        public string Text { get; set; }
        public virtual User Author { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}