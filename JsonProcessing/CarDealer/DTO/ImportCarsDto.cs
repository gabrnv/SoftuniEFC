using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    [JsonObject]
    public class ImportCarsDto
    {
        [JsonProperty("make")]
        string Make { get; set; }

        [JsonProperty("model")]
        string Model { get; set; }

        [JsonProperty("travelledDistance")]
        long TravelledDistance { get; set; }

        [JsonProperty("partsId")]

        List<int> PartsId { get; set; }

    }
}
