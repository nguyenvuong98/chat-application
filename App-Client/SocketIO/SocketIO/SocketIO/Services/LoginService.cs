using Newtonsoft.Json;
using SocketIO.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SocketIO.Services
{
    public interface ILoginService
    {
        Task<(bool ok, string message)> Login(string username);
    }
    public class LoginService : ILoginService
    {
        static HttpClient client = new HttpClient();

        public LoginService()
        {
            client.BaseAddress = new Uri("http://192.168.103.2:3000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<(bool ok, string message)> Login(string username)
        {
            try
            {
                User request = new User { UserName = username };
                HttpResponseMessage response = await client.PostAsJsonAsync("sign-in", request);
                var contents = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BaseReponseModel>(contents);
                return (
                    ok: result.Status,
                    message: result.Message
                    );
            }
            catch (Exception e)
            {
                return (false , string.Empty);
            }
            
        }
    }
}
