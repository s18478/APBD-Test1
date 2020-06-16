using System.Collections.Generic;

namespace Test2.Models
{
    public class MusicLabel
    {
        public int IdMusicLabel { get; set; }

        public string Name { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
