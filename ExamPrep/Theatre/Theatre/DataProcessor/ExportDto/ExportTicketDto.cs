using Newtonsoft.Json;

namespace Theatre.DataProcessor.ExportDto
{
    [JsonObject]
    public class ExportTicketDto
    {
        [JsonProperty("Price")]
        public decimal Price { get; set; }
        [JsonProperty("RowNumber")]
        public int RowNumber { get; set; }
    }
}