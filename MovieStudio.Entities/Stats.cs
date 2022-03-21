using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStudio.Entities
{
    public class Stats
    {
        public string MovieId { get; set; }
        public string WatchDurationMs { get; set; }
    }
}
