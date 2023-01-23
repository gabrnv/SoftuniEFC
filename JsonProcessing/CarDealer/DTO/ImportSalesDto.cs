using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    [JsonObject]
    public class ImportSalesDto
    {
        [JsonProperty("carId")]
        public int CarId { get; set; }

        [JsonProperty("customerId")]

        public int CustomerId { get; set; }

        [JsonProperty("discount")]

        public int Discount { get; set; }
    }
}
