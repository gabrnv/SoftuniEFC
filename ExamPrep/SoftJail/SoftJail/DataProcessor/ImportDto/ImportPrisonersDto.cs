namespace SoftJail.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ImportPrisonersDto
    {
        public ImportPrisonersDto()
        {
            Mails = new HashSet<ImportMailDto>(); 
        }
        [JsonProperty("FullName")]
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string FullName { get; set; }

        [JsonProperty("Nickname")]
        [Required]
        [RegularExpression(@"^(The\s)([A-z][a-z]*)$")]
        public string Nickname { get; set; }

        [JsonProperty("Age")]
        [Range(18,65)]
        [Required]
        public int Age { get; set; }

        [JsonProperty("IncarcerationDate")]
        [Required]
        public string IncarcerationDate { get; set; }

        [JsonProperty("ReleaseDate")]
        public string ReleaseDate { get; set; }

        [JsonProperty("Bail")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        [JsonProperty("CellId")]
        public int? CellId { get; set; }

        [JsonProperty("Mails")]
        public ICollection<ImportMailDto> Mails { get; set; }
    }
}
