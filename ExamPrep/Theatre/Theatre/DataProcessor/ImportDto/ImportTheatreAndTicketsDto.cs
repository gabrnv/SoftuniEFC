using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTheatreAndTicketsDto
    {
        [JsonProperty("Name")]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }

        [JsonProperty("NumberOfHalls")]
        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte NumberOfHalls { get; set; }

        [JsonProperty("Director")]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Director { get; set; }

        [JsonProperty("Tickets")] //////////
        public List<ImportTicketDto> Tickets { get; set; }
    }
}
