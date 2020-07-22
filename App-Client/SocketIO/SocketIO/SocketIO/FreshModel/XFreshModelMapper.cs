using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIO.FreshModel
{
    internal class XFreshPageModelMapper : FreshPageModelMapper, IFreshPageModelMapper
    {
        readonly static IDictionary<Type, Type> viewModelViewPairs = new Dictionary<Type, Type>();
        readonly static IDictionary<Type, Type> viewViewModelPairs = new Dictionary<Type, Type>();

        public static void RegisterPair<TViewModel, TView>()
            where TView : Xamarin.Forms.Page
            where TViewModel : FreshBasePageModel
        {
            RegisterPair(typeof(TViewModel), typeof(TView));
        }

        public static void RegisterPair(Type viewModelType, Type viewType)
        {
            viewModelViewPairs[viewModelType] = viewType;
            viewViewModelPairs[viewType] = viewModelType;
        }

        string IFreshPageModelMapper.GetPageTypeName(Type pageModelType)
        {
            return GetPageTypeName(pageModelType);
        }

        /// <summary>
        /// Get page model type associated with a given page type
        /// </summary>
        /// <param name="pageType"></param>
        /// <returns></returns>
        public static Type GetPageModelType(Type pageType)
        {
            return viewViewModelPairs.Get(pageType);
        }

        /// <summary>
        /// Get page model type associated with a given page type
        /// </summary>
        /// <param name="pageType"></param>
        /// <returns></returns>
        public static Type GetPageType(Type pageModelType)
        {
            return viewModelViewPairs.Get(pageModelType);
        }

        public new string GetPageTypeName(Type pageModelType)
        {
            var pageType = GetPageType(pageModelType);
            if (pageType != null)
            {
                return pageType.AssemblyQualifiedName;
            }

            return base.GetPageTypeName(pageModelType);
        }
    }

    static class DictionaryExtensions
    {
        public static Type Get(this IDictionary<Type, Type> dictionary, Type key)
        {
            if (dictionary == null || key == null) return null;

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            var actualKey = dictionary.Keys.FirstOrDefault(x => x.AssemblyQualifiedName == key.AssemblyQualifiedName);

            if (actualKey == null) return null;

            return dictionary[actualKey];
        }
    }
}
