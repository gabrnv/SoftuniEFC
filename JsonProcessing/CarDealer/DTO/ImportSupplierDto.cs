using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    [JsonObject]
    public class ImportSupplierDto
    {
        [JsonProperty("name")]
        string Name { get; set; } 

        [JsonProperty("isImporter")]
        bool IsImporter { get; set; }
    }
}
