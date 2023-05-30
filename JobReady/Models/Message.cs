﻿using System.ComponentModel.DataAnnotations;

namespace JobReady.Models
{
    public class Message
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string MessageText { get; set; }
        public DateTime CreatedOn { get; set; }
        public long SenderID { get; set; }

        public long ReceiverID { get; set; }
        public UserAccount Sender { get; set; }

        public UserAccount Receiver { get; set; }
    }
}