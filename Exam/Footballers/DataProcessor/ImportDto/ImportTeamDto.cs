using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Footballers.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTeamDto
    {
        [JsonProperty("Name")]
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^([A-z\d\s\.\-])+$")]
        public string Name { get; set; }
        [JsonProperty("Nationality")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality  { get; set; }
        [JsonProperty("Trophies")]
        [Required]
        public int Trophies { get; set; }
        [JsonProperty("Footballers")]
        public List<int> Footballers { get; set; }
    }
}
