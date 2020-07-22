using Newtonsoft.Json;
using Quobject.Collections.Immutable;
using Quobject.SocketIoClientDotNet.Client;
using SocketIO.BaseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SocketIO.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public string UserName { get; set; }
        public Socket Socket { get; set; }
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users {
            get => _users;
            set
            {
                if(value != _users)
                {
                    _users = value;
                    RaisePropertyChanged("Users");
                }
            }
        }
        public HomeViewModel()
        {
            Users = new ObservableCollection<User>();
            //ImmutableList<string> immutableList =  ImmutableList.Create<string>("WebSocket", "Flash Socket", "AJAX long-polling");
            //Socket = IO.Socket("http://192.168.103.2:3000", new IO.Options { Transports = immutableList });
            Socket = IO.Socket("http://192.168.103.2:3000");
            Socket.On("users", (data) => {
                var users = JsonConvert.SerializeObject(data);
                Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(users);
                UpdateUser();
            });
            Socket.On(Socket.EVENT_RECONNECT, () => {
                Socket.Emit("username", UserName);
            });

            ConnectCommand = new Command<User>(async (item) => await Connect(item));
        }
        public ICommand ConnectCommand { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            var data = (Dictionary<string, Object>)initData;
            if(data != null)
            {
                UserName = data["username"]?.ToString();
            }
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Init();
        }
        void Init()
        {
            
            Socket.On(Socket.EVENT_CONNECT, () =>
            {
                Socket.Emit("username", UserName);
            });
            
        }
        void UpdateUser()
        {
            if (Users?.Count <= 0) return;
            var user = Users.FirstOrDefault(x => x.UserName == UserName);
            user.IsMe = true;
            var users = Users.OrderByDescending(x => x.IsMe);
            Users = new ObservableCollection<User>(users);
        }
        async Task Connect(User item)
        {
            if (item.IsMe) return;
            if (IsBusy) return;
            IsBusy = true;
            await CoreMethods.PushPageModel<ChatViewModel>(
                new Dictionary<string, Object>
                {
                    {"username", UserName },
                    {"friend", item.UserName }
                }
                );
            IsBusy = false;
        }
    }
}
