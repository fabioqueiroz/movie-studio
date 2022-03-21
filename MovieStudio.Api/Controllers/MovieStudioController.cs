using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreLinq;
using MovieStudio.Dtos;
using MovieStudio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStudio.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieStudioController : ControllerBase
    {
        private readonly IMovieStudioService _movieStudioService;
        private List<MovieStudioDto> _database;
        public MovieStudioController(IMovieStudioService movieStudioService)
        {
            _movieStudioService = movieStudioService;
            _database = new List<MovieStudioDto>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("MovieStudio API working");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Metadata([FromBody] MovieStudioDto movie)
        {
            _database.Add(movie);
            return Ok();
        }

        [HttpGet]
        [Route("[action]/{movieId}")]
        public IActionResult Metadata(int movieId)
        {
            var result = _movieStudioService.GetMovieByMoviedId(movieId);
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{stats}")]
        public IActionResult Movies()
        {
            return Ok(_movieStudioService.GetAllMovieStats());
        }
    }
}
