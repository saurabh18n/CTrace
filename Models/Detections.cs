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
        public virtual User User { get; set; }

        [Required]
        [Column("detection_time")]
        public DateTime DetectedAt { get; set; }

        [Required]
        [Column("contact_createdby")]
        public virtual User CreatedBy { get; set; }
    }
}
