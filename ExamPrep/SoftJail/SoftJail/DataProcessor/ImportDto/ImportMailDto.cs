using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportMailDto
    {
        [JsonProperty("Description")]
        [Required]
        public string Description { get; set; }

        [JsonProperty("Sender")]
        [Required]
        public string Sender { get; set; }
        [JsonProperty("Address")]
        [RegularExpression(@"^([A-Za-z\s0-9]+?)(\sstr.)$")]
        public string Address { get; set; }
    }
}