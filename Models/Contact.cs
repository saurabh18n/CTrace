using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CTrace.Models
{
    [Table("tab_contact")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("contact_id")]
        public Guid id { get; set; }

        [Required]
        [Column("contact_primaryuserid")]
        public virtual User PrimaryUser { get; set; }

        [Required]
        [Column("contact_seconderyuserid")]
        public virtual User SecondUser { get; set; }

        [Required]
        [Column("contact_time")]
        public DateTime contact_created { get; set; }

        [Column("contact_createdby")]
        public virtual User CreatedBy { get; set; }
    }
}
