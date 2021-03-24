using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TGbot.Models
{
    [Table("Messages")]
    public class Message
    {


        public int Id { get; set; }

        [ForeignKey("User"), Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [StringLength(4096)]
        public string Text { get; set; }

        [DefaultValue("NOW()")]
        public DateTime Time { get; set; }



    }
}
