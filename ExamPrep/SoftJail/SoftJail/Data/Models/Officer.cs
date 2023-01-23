
namespace SoftJail.Data.Models
{
    using SoftJail.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Officer
    {
        public Officer()
        {
            OfficerPrisoners = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FullName { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public Position Position { get; set; }

        public Weapon Weapon { get; set; }

        //TODO: Implement Connections

        [Required]
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }

        [Required]
        public virtual Department Department { get; set; }

        public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }

    }
}
