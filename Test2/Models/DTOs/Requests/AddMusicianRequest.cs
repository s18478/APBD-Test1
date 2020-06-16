
using System.ComponentModel.DataAnnotations;

namespace Test2.Models.DTOs.Requests
{
    public class AddMusicianRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public Track Track { get; set; }
    }
}
