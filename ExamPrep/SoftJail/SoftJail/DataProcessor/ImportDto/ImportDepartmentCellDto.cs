namespace SoftJail.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportDepartmentCellDto
    {
        [JsonProperty("CellNumber")]
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        [JsonProperty("HasWindow")]
        public bool HasWindow { get; set; }
    }
}
