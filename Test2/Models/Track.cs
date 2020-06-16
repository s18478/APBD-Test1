using System.Collections.Generic;

namespace Test2.Models
{
    public class Track
    {
        public int IdTrack { get; set; }

        public string TrackName { get; set; }

        public float Duration { get; set; }

        public int? IdMusicAlbum { get; set; }

        public Album Album { get; set; }

        public ICollection<MusicianTrack> MusicianTracks { get; set; }
    }
}