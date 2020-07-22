using Newtonsoft.Json;
using Quobject.Collections.Immutable;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SocketIO.ViewModel
{
    public class ChatViewModel: BaseViewModel
    {
        public Socket Socket { get; set; }
        public string Friend { get; set; }
        public string Alert { get; set; } 
        public string Text { get; set; }
        private string _username;
        private string _roomName;

        private ObservableCollection<ChatContentModel> _chatContentModels;
        public ObservableCollection<ChatContentModel> ChatContentModels {
            get => _chatContentModels;
            set
            {
                if(value != _chatContentModels)
                {
                    _chatContentModels = value;
                    RaisePropertyChanged("ChatContentModels");
                }
            }
        }
        public ChatViewModel()
        {
            ChatContentModels = new ObservableCollection<ChatContentModel>();
            //ImmutableList<string> immutableList = ImmutableList.Create<string>("WebSocket", "Flash Socket", "AJAX long-polling");
            //Socket = IO.Socket("http://192.168.103.2:3000",new IO.Options {Transports = immutableList });
            Socket = IO.Socket("http://192.168.103.2:3000");
            Socket.On("connect-success", (data) => {
                Alert = "Bắt đầu trò chuyện";
                var room = JsonConvert.SerializeObject(data);
                Room _room = JsonConvert.DeserializeObject<Room>(room);
                RaisePropertyChanged("Alert");
                _roomName = _room.RoomName;
            });
            Socket.On("my-conversation", data => {
                var chat = JsonConvert.SerializeObject(data);
                var chatItem = JsonConvert.DeserializeObject<ChatContentModel>(chat);
                chatItem.IsMe = chatItem.Username == _username;
                Device.BeginInvokeOnMainThread(() => {
                    ChatContentModels.Add(chatItem);
                });
            });
            SubmitCommand = new Command(Submit);
        }
        public ICommand SubmitCommand { get; set; }
        void Submit() {
            if (IsBusy) return;
            IsBusy = true;
            var data = new ChatContentModel { Username = _username, RoomName = _roomName, Content = Text };
            var json = JsonConvert.SerializeObject(data);
            Socket.Emit("request-chat",JsonConvert.DeserializeObject(json));
            Text = string.Empty;
            IsBusy = false;
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            var data = (Dictionary<string, Object>)initData;
            if (data == null) return;

            if (data.ContainsKey("username"))
            {
                _username = data["username"]?.ToString();
                
            }
            if (data.ContainsKey("friend"))
            {
                Friend = data["friend"]?.ToString();
                Alert = string.Format("Đang chờ {0} kết nối ...", Friend);
            }
            
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Socket.Emit("username", _username);
            Socket.Emit("request-connect", Friend);
        }
    }
    public class Room
    {
        public string RoomName { get; set; }
    }
    public class ChatContentModel
    {
        public string Username { get; set; }
        public string RoomName { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool IsMe { get; set; }
        public bool IsNotMe => !IsMe;
    }
}
