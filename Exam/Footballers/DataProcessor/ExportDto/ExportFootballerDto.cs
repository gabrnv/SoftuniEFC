using Newtonsoft.Json;

namespace Footballers.DataProcessor.ExportDto
{
    [JsonObject]
    public class ExportFootballerDto
    {
        [JsonProperty("FootballerName")]
        public string FoorballerName { get; set; }
        [JsonProperty("ContractStartDate")]
        public string ContractStartDate { get; set; }
        [JsonProperty("ContractEndDate")]
        public string ContractEndDate { get; set; }
        [JsonProperty("BestSkillType")]
        public string BestSkillType { get; set; }
        [JsonProperty("PositionType")]
        public string PositionType { get; set; }
    }
}