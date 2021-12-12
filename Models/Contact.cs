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
        public Guid PrimaryUserId { get; set; }
        public virtual User PrimaryUser { get; set; }

        [Required]
        [Column("contact_seconderyuserid")]
        public Guid SecondUserId { get; set; }
        public virtual User SecondUser { get; set; }

        [Required]
        [Column("contact_time")]
        public DateTime Time { get; set; }

        [Required]
        [Column("contact_reported")]
        public DateTime Reported { get; set; }

        [Column("contact_createdbyid")]
        public Guid CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}

namespace CTrace.ViewModels
{
    public class ContactVM
    {
        public Guid Userid { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public bool Detected { get; set; }
    }
    public class ContStatus
    {
        public Guid userid { get; set; }
        public IEnumerable<ContactVM> Contacts { get; set; }
    }
}