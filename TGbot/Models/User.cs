using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TGbot.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string ChatId { get; set; }

        [StringLength(100)]
        public string NickName { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public int? WaitMessageId { get; set; }


    }
}
