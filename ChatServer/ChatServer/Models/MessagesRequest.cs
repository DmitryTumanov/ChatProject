using System.Collections.Generic;

namespace ChatServer.Models
{
    public class MessagesRequest
    {
        public List<Message> Messages { get; set; }
    }
}
