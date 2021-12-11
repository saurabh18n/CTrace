using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CTrace.Models
{
    [Table("tab_detection")]
    public class Detection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("detection_id")]
        public Guid id { get; set; }

        [Required]
        [Column("detection_user")]
<<<<<<< HEAD
        public Guid UserId { get; set; }
=======
>>>>>>> f0d7e8db4b0498926dd526398b8c1de19f1e521e
        public virtual User User { get; set; }

        [Required]
        [Column("detection_time")]
        public DateTime DetectedAt { get; set; }

        [Required]
        [Column("contact_createdby")]
<<<<<<< HEAD
        public Guid CreatedById { get; set; }
=======
>>>>>>> f0d7e8db4b0498926dd526398b8c1de19f1e521e
        public virtual User CreatedBy { get; set; }
    }
}
