using MovieStudio.Dtos;
using MovieStudio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStudio.Services.Interfaces
{
    public interface IMovieStudioService
    {
        IEnumerable<MovieStudioDto> GetMovieByMoviedId(int movieId);
        IEnumerable<StatsDto> GetAllMovieStats();
    }
}
