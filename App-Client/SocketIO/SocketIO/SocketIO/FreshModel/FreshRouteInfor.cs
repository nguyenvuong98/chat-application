using System;
using System.Collections.Generic;
using System.Text;

namespace SocketIO.FreshModel
{
    public struct FreshRouteInfo
    {
        public string Route { get; set; }

        public Type ViewModelType { get; set; }

        public Type ViewType { get; set; }

        public bool IsValid()
        {
            return ViewModelType != null
                && ViewType != null
                && false == string.IsNullOrWhiteSpace(Route);
        }

        public override string ToString()
        {
            return $"vm {ViewModelType}  v {ViewType} r {Route}";
        }
    }
}
