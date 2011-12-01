using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StegoCore.Infra
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            return IoC.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return IoC.ResolveAll(serviceType);
        }
    }
}