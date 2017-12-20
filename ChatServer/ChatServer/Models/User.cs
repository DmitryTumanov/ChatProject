using System.ComponentModel.DataAnnotations;

namespace ChatServer.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string ConnectionId { get; set; }

        public string UserName { get; set; }
    }
}