using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzDemo.EF.Entity
{
    [Table("TB_User")]
    public class TB_User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(20),Required]
        public string Name { get; set; }
        [MaxLength(16),Required]
        public string Pwd { get; set; }
        [MaxLength(16),Required]
        public string Email { get; set; }
        public DateTime RegistTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public bool Status { get; set; }
    }
}
