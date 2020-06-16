using System;
using System.Collections.Generic;


namespace Test2.Models.DTOs.Responses
{
    public class GetMusicianResponse
    {
        public Musician Musician { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}
