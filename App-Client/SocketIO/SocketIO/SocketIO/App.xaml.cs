using FreshMvvm;
using SocketIO.FreshModel;
using SocketIO.Services;
using SocketIO.View;
using SocketIO.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocketIO
{
    public partial class App : Application
    {
        public App(Action<IFreshIOC> regiserPlatformServices)
        {
            InitializeComponent();
            
            FreshRouting.RegisterRoute<SignInViewModel,SignInPage>("sign-in");
            FreshRouting.RegisterRoute<HomeViewModel,HomePage>("home");
            FreshRouting.RegisterRoute<RoomViewModel,RoomPage>("room");
            FreshRouting.RegisterRoute<ChatViewModel, ChatPage>("chat");


            RegisterServices(FreshIOC.Container, regiserPlatformServices);
            var page = FreshPageModelResolver.ResolvePageModel<SignInViewModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }
        void RegisterServices(IFreshIOC container, Action<IFreshIOC> regiserPlatformServices) {

            regiserPlatformServices?.Invoke(container);
            //DependencyService.Register<DependencyGetter>();

            container.Register<ILoginService, LoginService>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
    class DependencyGetter
    {
        public T Get<T>()
        {
            return (T)FreshIOC.Container.Resolve(typeof(T));
        }
    }
}
