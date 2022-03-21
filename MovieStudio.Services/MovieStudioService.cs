using CsvHelper;
using CsvHelper.Configuration;
using MoreLinq;
using MovieStudio.Dtos;
using MovieStudio.Entities;
using MovieStudio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MovieStudio.Services
{
    public class MovieStudioService : IMovieStudioService
    {
        public IEnumerable<MovieStudioDto> GetMovieByMoviedId(int movieId)
        {
            IEnumerable<Movie> records;
            var result = new List<MovieStudioDto>();

            using (var reader = new StreamReader(@"C:\Users\fabio\OneDrive\Desktop\All Projects\C#\MovieStudio.Api\MovieStudio.Api\Data\metadata.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Movie>().Where(x => x.MovieId == movieId && !string.IsNullOrEmpty(x.Language)
                                                       && !string.IsNullOrEmpty(x.Title) && !string.IsNullOrEmpty(x.Duration)
                                                       && x.ReleaseYear > 0)
                                                      .OrderBy(x => x.Language).ThenByDescending(x => x.Id).DistinctBy(x => x.Language).ToList();

                if (records.Any())
                {
                    records.ForEach(x => result.Add(new MovieStudioDto 
                    { 
                        MovieId = x.MovieId, 
                        Title = x.Title, 
                        Duration = x.Duration, 
                        Language = x.Language,
                        ReleaseYear = x.ReleaseYear
                    }));
                }
            }

            return result;           
        }

        public IEnumerable<StatsDto> GetAllMovieStats() // Needs to be optmised
        {
            var records = new List<Stats>();
            var statsList = new List<StatsDto>();
            
            using (var reader = new StreamReader(@"C:\Users\fabio\OneDrive\Desktop\All Projects\C#\MovieStudio.Api\MovieStudio.Api\Data\stats.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    records.Add(new Stats { MovieId = values[0], WatchDurationMs = values[1] });
                }
            }

            records.ForEach(x => statsList.Add(new StatsDto 
            { 
                MovieId = int.Parse(x.MovieId), 
                AverageWatchDurationS = int.Parse(x.WatchDurationMs)/1000
            }));

            var result = statsList.GroupBy(x => x.MovieId).Select(s => 
            {
                var movie = GetMovieByMoviedId(s.Key)?.ToList().FirstOrDefault() ?? new MovieStudioDto();

                return new StatsDto
                {
                    MovieId = s.Key,
                    AverageWatchDurationS = s.Sum(s => s.AverageWatchDurationS),
                    Watches = s.Sum(s => s.MovieId),
                    Title = movie?.Title ?? string.Empty,
                    ReleaseYear = movie.ReleaseYear > 0 ? movie.ReleaseYear : 0
                };
            });

            return result;
        }
    }
}
