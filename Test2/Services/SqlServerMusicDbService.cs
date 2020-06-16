
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Test2.Exceptions;
using Test2.Models;
using Test2.Models.DTOs.Requests;
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

        public void AddMusician(AddMusicianRequest request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (request.Track != null)
                    {
                        var track = GetTrackByName(request.Track.TrackName);

                        if (track == null)
                        {
                            _context.Add(request.Track);
                            _context.SaveChanges();
                        }
                    }

                    var trackId = GetTrackByName(request.Track.TrackName).IdTrack;

                    var Musician = new Musician
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Nickname = request.Nickname
                    };

                    _context.Musicians.Add(Musician);

                    var MusicianTrack = new MusicianTrack
                    {
                        IdTrack = trackId,
                        IdMusician = _context.Musicians
                                .Where(p => p.FirstName == Musician.FirstName && p.LastName == Musician.LastName)
                                .FirstOrDefault().IdMusician

                    };

                    _context.MusicianTracks.Add(MusicianTrack);
                    _context.SaveChanges();

                    transaction.Commit();
                } catch (Exception exc) 
                {
                    transaction.Rollback();
                    throw new MusicianDoesNotAddedException("Uncorrect requst!");
                }
            }
                
        }

        public Track GetTrackByName(string trackName)
        {
            return _context.Tracks.SingleOrDefault(p => p.TrackName.Equals(trackName));
        }
    } 
}
