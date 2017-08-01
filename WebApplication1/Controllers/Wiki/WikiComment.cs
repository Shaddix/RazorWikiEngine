using System;

namespace RuPM.Models.Database
{
    public class WikiComment
    {
        public int Id { get; set; }
        public WikiPage WikiPage { get; set; }
        public string Text { get; set; }
        //public string Author { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}