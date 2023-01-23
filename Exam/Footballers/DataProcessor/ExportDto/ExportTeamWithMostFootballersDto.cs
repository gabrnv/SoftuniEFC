using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.DataProcessor.ExportDto
{
    [JsonObject]
    public class ExportTeamWithMostFootballersDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Footballers")]
        public List<ExportFootballerDto> Footballers { get; set; }
    }
}
