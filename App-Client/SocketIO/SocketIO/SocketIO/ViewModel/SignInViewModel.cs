using Newtonsoft.Json;
using SocketIO.BaseModel;
using SocketIO.Services;
using SocketIO.View;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SocketIO.ViewModel
{
    public class SignInViewModel: ViewModelBase
    {
        static HttpClient client = new HttpClient();
        private ILoginService _loginService;
        public string Username { get; set; }
        public SignInViewModel(ILoginService loginService)
        {
            _loginService = loginService;
            SignInCommand = new Command(async () => await SignIn());
            client.BaseAddress = new Uri("http://192.168.103.2:3000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ICommand SignInCommand { get; set; }

        async Task SignIn()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                var response = await _loginService.Login(Username).ConfigureAwait(false);
                if (response.ok)
                {
                    Device.BeginInvokeOnMainThread(async () => {
                        await CoreMethods.PushPageModel<HomeViewModel>(
                            new Dictionary<string, Object>
                            {
                                {"username", Username }
                            }
                        );
                    });
                     
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () => {
                        await CoreMethods.DisplayAlert("Thông báo", "Tên đăng nhập đã tồn tại", "Đồng ý");
                    });
                    //await Application.Current.MainPage.DisplayAlert("Thông báo", "Tên đăng nhập đã tồn tại", "Đồng ý");
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
