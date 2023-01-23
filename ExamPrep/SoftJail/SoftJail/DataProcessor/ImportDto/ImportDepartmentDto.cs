namespace SoftJail.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ImportDepartmentDto
    {
        public ImportDepartmentDto()
        {
            Cells = new HashSet<ImportDepartmentCellDto>();
        }

        [JsonProperty("Name")]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [JsonProperty("Cells")]
        public ICollection<ImportDepartmentCellDto> Cells { get; set; }

    }
}
