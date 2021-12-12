using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CTrace.Models
{
    [Table("tab_users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public Guid user_id { get; set; }

        [Column("user_name"),Required]
        public string user_name { get; set; }

        [MinLength(10),MaxLength(10)]
        [Column("user_mobile"),Required]
        public string user_mobile { get; set; }

        [Column("user_isadmin"), Required]
        public bool? user_isadmin { get; set; }

        [Column("user_salt"), Required,StringLength(64)]
        public string user_salt { get; set; }

        [Column("user_pass"),Required, StringLength(64)]
        public string user_pass { get; set; }

        [Column("user_failcount"), Required]
        public byte user_failcount { get; set; }

        [Column("user_lastlogin"), Required]
        public DateTime user_lastlogin { get; set; }
    }
}
