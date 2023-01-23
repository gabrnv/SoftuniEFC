
namespace SoftJail.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Cell
    {
        public Cell()
        {
            Prisoners = new HashSet<Prisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }

        //TODO:Implement Connections

        [Required]
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set;}

        [Required]
        public virtual Department Department { get; set; }

        public virtual ICollection<Prisoner> Prisoners { get; set; }
    }
}
