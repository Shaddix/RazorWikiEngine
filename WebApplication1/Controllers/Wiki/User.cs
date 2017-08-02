using System.ComponentModel.DataAnnotations;

namespace RuPM.Models.Database
{
    public class User
    {
        [Key]
        public string Login { get; set; }
    }
}