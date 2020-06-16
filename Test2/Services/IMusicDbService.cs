using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test2.Models;
using Test2.Models.DTOs.Responses;

namespace Test2.Services
{
    interface IMusicDbService
    {
        GetMusicianResponse GetMusician(int id);
    }
}
