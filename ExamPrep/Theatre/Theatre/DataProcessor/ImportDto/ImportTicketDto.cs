using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTicketDto
    {
        [JsonProperty("Price")]
        [Required]
        [Range(typeof(decimal), "1.00", "100.00")]
        public decimal Price { get; set; }
        [JsonProperty("RowNumber")]
        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }
        [JsonProperty("PlayId")]
        [Required]
        public int PlayId { get; set; }
    }
}
