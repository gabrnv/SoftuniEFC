namespace SoftJail.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OfficerPrisoner
    {
        [ForeignKey(nameof(Prisoner))]
        public int PrisonerId { get; set; }
        
        [Required]
        public virtual Prisoner Prisoner { get; set; }

        [ForeignKey(nameof(Officer))]
        public int OfficerId { get; set; }

        [Required]
        public virtual Officer Officer { get; set; }
    }
}
