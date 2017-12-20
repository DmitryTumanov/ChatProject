using System.Linq;
using ChatServer.Models;

namespace ChatServer.Repository
{
    public class AccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository()
        {
            _context = new ApplicationDbContext();
        }

        public bool? CheckAccount(string name, string password)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Name == name);
            if (user != null)
            {
                return user.Password == password;
            }
            return null;
        }

        public void CreateLogin(string userName, string password)
        {
            _context.Accounts.Add(new Account {Name = userName, Password = password});
            _context.SaveChanges();
        }

        public bool IsLoginExisted(string login)
        {
            return _context.Accounts.Any(x => x.Name == login);
        }
    }
}