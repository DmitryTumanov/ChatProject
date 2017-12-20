using System;
using System.ComponentModel.DataAnnotations;

namespace ChatServer.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string FirstConnectionId { get; set; }
        public string SecondConnectionId { get; set; }
        public DateTime Date { get; set; }
        public string MessageText { get; set; }
        public int? CipherType { get; set; }
    }
}