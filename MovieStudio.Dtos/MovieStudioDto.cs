using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStudio.Dtos
{
    public class MovieStudioDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public int ReleaseYear { get; set; }
    }
}
