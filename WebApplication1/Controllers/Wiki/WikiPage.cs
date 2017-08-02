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
        public virtual IList<WikiTag> Tags { get; set; }
        public virtual IList<WikiComment> Comments { get; set; }
        public bool IsLayout { get; set; }

        [ForeignKey(nameof(LayoutPageId))]
        public virtual WikiPage LayoutPage { get; set; }
        public int? LayoutPageId { get; set; }
        public bool StickGlobal { get; set; }
        public bool StickCategory { get; set; }

        /// <summary>
        /// System pages are not shown to the visitor
        /// </summary>
        public bool IsSystemPage { get; set; }

        public virtual User Author { get; set; }

        public WikiPage()
        {
            ChangedDate = DateTimeOffset.Now;
            CreatedDate = DateTimeOffset.Now;
            Tags = new List<WikiTag>();
            Comments = new List<WikiComment>();
        }
    }
}