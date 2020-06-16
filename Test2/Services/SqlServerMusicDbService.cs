
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Test2.Exceptions;
using Test2.Models;
using Test2.Models.DTOs.Responses;

namespace Test2.Services
{
    public class SqlServerMusicDbService : IMusicDbService
    {
        private readonly MusicDbContext _context;

        public SqlServerMusicDbService(MusicDbContext context)
        {
            _context = context;
        }

        public GetMusicianResponse GetMusician(int id)
        {
            var musician = _context.Musicians
                             .Include(m => m.MusicianTracks)
                             .FirstOrDefault(m => m.IdMusician == id);

            if (musician == null)
            {
                throw new MusicianDoesNotExistsException("Musician does not exists!");
            }

            var tracks = _context.MusicianTracks
                        .Where(p => p.IdMusician == id)
                        .Include(p => p.Track)
                        .Select(p => p.Track)
                        .ToList();
                        

            var response = new GetMusicianResponse
            {
                Musician = musician,
                Tracks = tracks
            };

            return response;
        }
    }
}
