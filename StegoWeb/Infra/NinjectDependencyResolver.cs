using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using StegoCore;
using StegoCore.Facebook;
using StegoCore.Json;
using StegoCore.Stego;

namespace StegoWeb.Infra
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IFacebookApiService>().To<FacebookApiService>();
            _kernel.Bind<ISerializationService>().To<JsonSerializationService>();
            _kernel.Bind<ISteganographyService>().To<SteganographyService>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}