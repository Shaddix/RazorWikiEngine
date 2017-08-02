using System;

namespace RuPM.Models.Database
{
    public class WikiPageHistory
    {
        public int Id { get; set; }
        public WikiPage WikiPage { get; set; }
        public DateTimeOffset ChangedDate { get; set; }

        public string PageTitle { get; set; }
        public string Content { get; set; }
    }
}