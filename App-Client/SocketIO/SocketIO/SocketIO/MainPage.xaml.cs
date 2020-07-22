using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SocketIO
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public Socket Socket { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public MainPage()
        {
            InitializeComponent();
            Init();
            Users = new ObservableCollection<User>();
            AddData();
            Socket.On("users", (data) => {
                var users = JsonConvert.SerializeObject(data);
                Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(users);
                var a = 1;
               //JsonConvert.DeserializeAnonymousType<ObservableCollection<User>>((string)data, Users);
            });
        }
        void Init()
        {
            Socket = IO.Socket("http://192.168.103.2:3000");
            Socket.On(Socket.EVENT_CONNECT, () => {
                Socket.Emit("hi");
            });
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Socket.Emit("sign-in", username.Text);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Users.Add(new User { UserName = "a" });
        }
        void AddData()
        {
            Users.Add(new User { UserName = "a" });
            Users.Add(new User { UserName = "b" });
            Users.Add(new User { UserName = "c" });
            Users.Add(new User { UserName = "d" });
        }
    }
    public class User
    {
        public string UserName { get; set; }
        public bool IsOnline { get; set; }
        public bool IsMe { get; set; }
    }
}
