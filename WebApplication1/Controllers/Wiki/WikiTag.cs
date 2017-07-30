using System.ComponentModel.DataAnnotations;

namespace RuPM.Models.Database
{
    public class WikiTag
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string TagForLink { get; set; }
        public string FullTag { get; set; }
    }
}