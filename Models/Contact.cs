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
<<<<<<< HEAD
        public Guid PrimaryUserId { get; set; }
=======
>>>>>>> f0d7e8db4b0498926dd526398b8c1de19f1e521e
        public virtual User PrimaryUser { get; set; }

        [Required]
        [Column("contact_seconderyuserid")]
<<<<<<< HEAD
        public Guid SecondUserId { get; set; }
=======
>>>>>>> f0d7e8db4b0498926dd526398b8c1de19f1e521e
        public virtual User SecondUser { get; set; }

        [Required]
        [Column("contact_time")]
<<<<<<< HEAD
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
        public string Detected { get; set; }
    }
}
=======
        public DateTime contact_created { get; set; }

        [Column("contact_createdby")]
        public virtual User CreatedBy { get; set; }
    }
}
>>>>>>> f0d7e8db4b0498926dd526398b8c1de19f1e521e
