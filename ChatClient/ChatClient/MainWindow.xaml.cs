using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.AspNet.SignalR.Client;
using _1Lab;
using _2Lab;

namespace ChatClient
{
    public partial class MainWindow
    {
        private HubConnection _userConnection;
        private IHubProxy _hubProxy;
        private ICipher _cipher;

        private List<User> _usersList;
        private List<Message> _messages;

        public MainWindow()
        {
            ConnectToHubs();
            InitializeComponent();
            LockAll();
            UnlockUserName();
        }

        private void ConnectToHubs()
        {
            _userConnection = new HubConnection("http://localhost:37721/signalr/hubs");
            _hubProxy = _userConnection.CreateHubProxy("UserHub");
            _hubProxy.On<AllUsersRequest>("GetUsersList", GetUsersList);
            _hubProxy.On<MessagesRequest>("GetMessages", GetMessages);
            _hubProxy.On<Message>("GetSendMessage", GetSendMessage);
            _hubProxy.On<bool>("CheckLogin", CheckLogin);
            _hubProxy.On("ErrorLogin", ErrorLogin);
            _userConnection.Start().Wait();
        }

        private void CheckLogin(bool isLoginExisted)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SaveUserNameButton.Content = isLoginExisted ? "Login" : "Register";
            }));
        }

        private void ErrorLogin()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show("Invalid credentials!");
            }));
        }

        private void GetSendMessage(Message data)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (ActualUsers.SelectedIndex < 0)
                {
                    return;
                }
                var user = _usersList[ActualUsers.SelectedIndex];
                if (user.ConnectionId != data.FirstConnectionId)
                {
                    return;
                }
                UpdateCipher(data.CipherType);
                data.MessageText = _cipher.Decryption(data.MessageText);
                _messages.Add(data);
                foreach (var message in _messages)
                {
                    if (message.FirstConnectionId == _userConnection.ConnectionId)
                    {
                        message.FirstConnectionId = null;
                    }
                }
                ActualMessages.ItemsSource = null;
                ActualMessages.ItemsSource = _messages;
            }));
        }

        private void GetUsersList(AllUsersRequest request)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var user = ActualUsers.SelectedIndex >= 0 ? _usersList[ActualUsers.SelectedIndex] : null;
                _usersList = request.Users;
                var currentUser = _usersList.FirstOrDefault(x => x.UserName == CurrentUserName.Text);
                _usersList.Remove(currentUser);
                ActualUsers.ItemsSource = null;
                ActualUsers.ItemsSource = _usersList;
                if (user != null)
                {
                    var index = _usersList.Select(x => x.UserName).ToList().IndexOf(user.UserName);
                    ActualUsers.SelectedIndex = index;
                }
                LockAll();
                UnlockMessagingForms();
            }));
        }

        private void GetMessages(MessagesRequest request)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _messages = request.Messages;
                foreach (var message in _messages)
                {
                    if (message.FirstConnectionId == _userConnection.ConnectionId)
                    {
                        message.FirstConnectionId = null;
                    }
                    UpdateCipher(message.CipherType);
                    message.MessageText = _cipher.Decryption(message.MessageText);
                }
                ActualMessages.ItemsSource = null;
                ActualMessages.ItemsSource = _messages;
            }));
        }

        private void UpdateCipher(int? messageCipherType)
        {
            if (!messageCipherType.HasValue)
            {
                return;
            }
            switch ((CipherTypes)messageCipherType)
            {
                case CipherTypes.Cesar:
                    _cipher = new CesarCipher();
                    break;
                case CipherTypes.Des:
                    _cipher = new DesCipher();
                    break;
                case CipherTypes.TripleDes:
                    _cipher = new TripleDes();
                    break;
                case CipherTypes.TripleDesTwo:
                    _cipher = new TripleDoubleKeyDes();
                    break;
            }
        }

        private void SaveCurrentUserName(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentUserName.Text))
            {
                return;
            }
            var userName = CurrentUserName.Text;
            _hubProxy.Invoke("Connect", _userConnection.ConnectionId, userName, CurrentUserPassword.Password).Wait();
        }

        private void UnlockMessagingForms()
        {
            CipherTypeList.IsEnabled =
                ActualMessages.IsEnabled =
                    ActualUsers.IsEnabled =
                        SendButton.IsEnabled =
                            MessageArea.IsEnabled = true;
        }

        private void UnlockUserName()
        {
            CurrentUserName.IsEnabled = SaveUserNameButton.IsEnabled = CurrentUserPassword.IsEnabled = true;
        }

        private void LockAll()
        {
            CipherTypeList.IsEnabled =
                ActualMessages.IsEnabled =
                    ActualUsers.IsEnabled =
                        CurrentUserName.IsEnabled =
                            SaveUserNameButton.IsEnabled =
                                CurrentUserPassword.IsEnabled =
                                    SendButton.IsEnabled =
                                        MessageArea.IsEnabled = false;
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentUserName.Text))
            {
                return;
            }
            var userName = CurrentUserName.Text;
            //_hubProxy.Invoke("Disconnect", _userConnection.ConnectionId, userName).Wait();
        }

        private void ActualUsers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActualUsers.SelectedIndex < 0)
            {
                ActualMessages.ItemsSource = null;
                ActualMessages.ItemsSource = new List<Message>();
                return;
            }
            var user = _usersList[ActualUsers.SelectedIndex];
            _hubProxy.Invoke("GetMessages", _userConnection.ConnectionId, user.ConnectionId).Wait();
        }

        private void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MessageArea.Text) || ActualUsers.SelectedIndex < 0)
            {
                return;
            }
            var text = MessageArea.Text;
            var user = _usersList[ActualUsers.SelectedIndex];
            _messages.Add(new Message
            {
                Date = DateTime.Now,
                FirstConnectionId = null,
                MessageText = text,
                SecondConnectionId = user.ConnectionId
            });
            ActualMessages.ItemsSource = null;
            ActualMessages.ItemsSource = _messages;
            MessageArea.Text = "";
            try
            {
                UpdateCipher(CipherTypeList.SelectedIndex);
                _hubProxy.Invoke("SendMessage", _userConnection.ConnectionId, user.ConnectionId,
                    _cipher.Encryption(text), CipherTypeList.SelectedIndex).Wait();
            }
            catch
            {
                MessageBox.Show("user is offline");
            }
        }

        private void CurrentUserName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var login = CurrentUserName.Text;
            if (string.IsNullOrEmpty(login))
            {
                return;
            }
            _hubProxy.Invoke("CheckLogin", _userConnection.ConnectionId, login).Wait();
        }

        private void ActualMessages_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ActualMessages.SelectedIndex < 0)
            {
                return;
            }
            var message = _messages[ActualMessages.SelectedIndex];
            MessageBox.Show(message.MessageText);
        }
    }
}
