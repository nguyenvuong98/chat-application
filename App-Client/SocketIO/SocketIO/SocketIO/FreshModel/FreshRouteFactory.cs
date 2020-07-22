using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SocketIO.FreshModel
{
    public static partial class FreshRouting
    {
        static bool initialized = false;
        public static void Init()
        {
            if (initialized) return;

            initialized = true;
            FreshPageModelResolver.PageModelMapper = new XFreshPageModelMapper();
        }

        /// <summary>
        /// Register a new route with given route info
        /// </summary>
        /// <param name="routeInfo">The route</param>
        public static void RegisterRoute(FreshRouteInfo routeInfo)
        {
            if (false == routeInfo.IsValid()) return;

            XFreshPageModelMapper.RegisterPair(routeInfo.ViewModelType, routeInfo.ViewType);
            RegisterRoute(routeInfo.Route, routeInfo.ViewModelType);
        }

        /// <summary>
        /// Register a new route with strongly types for View and ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">The type of ViewModel</typeparam>
        /// <typeparam name="TView">The type of View</typeparam>
        /// <param name="route">The route</param>
        public static void RegisterRoute<TViewModel, TView>(string route)
            where TView : Xamarin.Forms.Page
            where TViewModel : FreshBasePageModel
        {
            XFreshPageModelMapper.RegisterPair<TViewModel, TView>();
            RegisterRoute<TViewModel>(route);
        }

        /// <summary>
        /// Register a new route with strongly type for ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">The type of ViewModel</typeparam>
        /// <param name="route">The route</param>
        public static void RegisterRoute<TViewModel>(string route)
            where TViewModel : FreshBasePageModel
        {
            RegisterRoute(route, typeof(TViewModel));
        }

        /// <summary>
        /// Register a new route with strongly type for ViewModel
        /// </summary>
        /// <param name="viewModelType">The type of ViewModel</param>
        /// <param name="route">The route</param>
        public static void RegisterRoute(string route, Type viewModelType)
        {
            Init();
            Routing.UnRegisterRoute(route);
            Routing.RegisterRoute(route, new FreshRoutingFactory(viewModelType));
        }

        class FreshRoutingFactory : Xamarin.Forms.RouteFactory
        {
            private readonly Type viewModelType;

            public FreshRoutingFactory(Type viewModelType)
            {
                this.viewModelType = viewModelType;
            }

            public override Xamarin.Forms.Element GetOrCreate()
            {
                return FreshPageModelResolver.ResolvePageModel(viewModelType, null);
            }
        }
    }
}
