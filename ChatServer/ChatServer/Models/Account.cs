using System.ComponentModel.DataAnnotations;

namespace ChatServer.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public int xc
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}