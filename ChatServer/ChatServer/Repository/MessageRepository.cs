using System.Collections.Generic;
using System.Linq;
using ChatServer.Models;

namespace ChatServer.Repository
{
    public class MessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository()
        {
            _context = new ApplicationDbContext();
        }

        public List<Message> GetMessages(string firstId, string secondId)
        {
            var firstList =
                _context.Messages.Where(x => x.FirstConnectionId == firstId && secondId == x.SecondConnectionId)
                    .ToList();
            var secondList =
                _context.Messages.Where(x => x.FirstConnectionId == secondId && firstId == x.SecondConnectionId)
                    .ToList();
            firstList.AddRange(secondList);
            return firstList.OrderBy(x => x.Date).ToList();
        }

        public void SaveMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }
    }
}