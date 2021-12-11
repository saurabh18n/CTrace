using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CTrace.Models
{
    [Table("tab_notification")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("notif_id")]
        public Guid notif_id { get; set; }

        [Required]
        [Column("notif_user")]
<<<<<<< HEAD
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
=======
        public virtual User notif_user { get; set; }
>>>>>>> f0d7e8db4b0498926dd526398b8c1de19f1e521e

        [Required]
        [Column("notif_text")]
        public string notif_text { get; set; }
        

        [Column("notif_redirect")]
        public string notif_redirect { get; set; }

        [Required]
        [Column("notif_created")]
        public DateTime notif_created { get; set; }

<<<<<<< HEAD
=======

>>>>>>> f0d7e8db4b0498926dd526398b8c1de19f1e521e
        [Column("notif_ṛead")]
        public DateTime notif_ṛead { get; set; }
    }
}
