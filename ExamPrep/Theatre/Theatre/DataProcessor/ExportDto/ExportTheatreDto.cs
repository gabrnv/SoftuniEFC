using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ExportDto
{
    [JsonObject]
    public class ExportTheatreDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Halls")]
        public int Halls { get; set; }
        [JsonProperty("TotalIncome")]
        public decimal TotalIncome { get; set; }
        [JsonProperty("Tickets")]
        public List<ExportTicketDto> Tickets { get; set; }
    }
}
