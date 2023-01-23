
namespace SoftJail.DataProcessor.ImportDto
{
    using SoftJail.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using System.Xml.Serialization;
    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        [XmlElement("Name")]
        [Required]
        public string FullName { get; set; }
        [XmlElement("Money")]
        [Required]
        [Range(typeof(decimal), "0", "7228162514264337593543950335")]
        public decimal Salary { get; set; }
        [XmlElement("Position")]
        [Required]
        public string Position { get; set; }
        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; }
        [XmlElement("DepartmentId")]
        [Required]
        public int DepartmentId { get; set; }
        [XmlArray("Prisoners")]
        public ImportOfficerPrisonerDto[] Prisoners { get; set; }
    }
}
