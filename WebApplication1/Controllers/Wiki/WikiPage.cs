using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuPM.Models.Database
{
    public class WikiPage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }

        public DateTimeOffset ChangedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Content { get; set; }
        public string ViewPath { get; set; }
        public string ViewUrl { get; set; }
        public IList<WikiTag> Tags { get; set; }
        public bool IsLayout { get; set; }

        [ForeignKey(nameof(LayoutPageId))]
        public WikiPage LayoutPage { get; set; }
        public int? LayoutPageId { get; set; }

        public WikiPage()
        {
            ChangedDate = DateTimeOffset.Now;
            CreatedDate = DateTimeOffset.Now;
        }
    }
}