﻿namespace ChatClient
{
    public class User
    {
        public int Id { get; set; }

        public string ConnectionId { get; set; }

        public string UserName { get; set; }

        public int UnreadMessagesCount { get; set; }
    }
}