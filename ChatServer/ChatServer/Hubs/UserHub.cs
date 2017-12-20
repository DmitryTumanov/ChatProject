using System;
using System.Threading.Tasks;
using System.Web.WebSockets;
using ChatServer.Models;
using ChatServer.Repository;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatServer.Hubs
{
    [HubName("UserHub")]
    public class UserHub : Hub
    {
        private readonly UserRepository _userRepository;
        private readonly MessageRepository _messageRepository;
        private readonly AccountRepository _accountRepository;

        public UserHub()
        {
            _userRepository = new UserRepository();
            _messageRepository = new MessageRepository();
            _accountRepository = new AccountRepository();     
        }

        public void Connect(string connectionId, string userName, string password)
        {
            var check = _accountRepository.CheckAccount(userName, password);
            if (check == null)
            {
                _accountRepository.CreateLogin(userName, password);
                check = true;
            }
            if (!check.Value)
            {
                Clients.Client(connectionId).ErrorLogin();
                return;
            }
            var result = _userRepository.AddUser(connectionId, userName);
            if (result)
            {
                Clients.All.GetUsersList(new AllUsersRequest {Users = _userRepository.GetUsers()});
            }
        }

        public void Disconnect(string connectionId, string userName)
        {
            _userRepository.DeleteUser(connectionId, userName);
        }

        public void GetMessages(string currentUser, string secondUser)
        {
            Clients.Client(currentUser).GetMessages(new MessagesRequest
            {
                Messages = _messageRepository.GetMessages(currentUser, secondUser)
            });
        }

        public void SendMessage(string currentUser, string secondUser, string text, int cipherType)
        {
            var model = new Message
            {
                Date = DateTime.Now,
                FirstConnectionId = currentUser,
                SecondConnectionId = secondUser,
                MessageText = text,
                CipherType = cipherType
            };
            _messageRepository.SaveMessage(model);
            Clients.Client(secondUser).GetSendMessage(model);
            Clients.Client(currentUser).GetSendMessage(model);
        }

        public void CheckLogin(string connectionId, string login)
        {
            Clients.Client(connectionId).CheckLogin(_accountRepository.IsLoginExisted(login));
        }
    }
}