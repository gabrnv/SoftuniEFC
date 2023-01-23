using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Footballers.Data.Models
{
    public class Coach
    {
        public Coach()
        {
            Footballers = new List<Footballer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]

        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Nationality { get; set; }

        public virtual ICollection<Footballer> Footballers { get; set; }   
    }
}
