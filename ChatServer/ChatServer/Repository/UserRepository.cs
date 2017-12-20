using System.Collections.Generic;
using System.Linq;
using ChatServer.Models;

namespace ChatServer.Repository
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository()
        {
            _context = new ApplicationDbContext();
        }

        public bool AddUser(string connectionId, string userName)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                var messages1 =
                    _context.Messages.Where(x => x.FirstConnectionId == user.ConnectionId);
                foreach (var message in messages1)
                {
                    message.FirstConnectionId = connectionId;
                }
                var messages2 =
                    _context.Messages.Where(x => x.SecondConnectionId == user.ConnectionId);
                foreach (var message in messages2)
                {
                    message.SecondConnectionId = connectionId;
                }
                user.ConnectionId = connectionId;
                _context.SaveChanges();
                return true;
            }
            if (_context.Users.Any(x => x.UserName == userName && x.ConnectionId == connectionId))
            {
                return false;
            }
            _context.Users.Add(new User
            {
                ConnectionId = connectionId,
                UserName = userName
            });
            _context.SaveChanges();
            return true;
        }

        public void DeleteUser(string connectionId, string userName)
        {
            var model = _context.Users.FirstOrDefault(x => x.UserName == userName && x.ConnectionId == connectionId);
            if (model == null)
            {
                return;
            }
            _context.Users.Remove(model);
            _context.SaveChanges();
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}