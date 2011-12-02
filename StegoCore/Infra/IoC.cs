using System.Collections.Generic;
using Ninject;
using StegoCore.Crypto;
using StegoCore.Facebook;
using StegoCore.Json;
using StegoCore.Stego;
using System;

namespace StegoCore.Infra
{
    public class IoC
    {
        private static readonly IKernel _kernel;

         static IoC()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IFacebookApiService>().To<FacebookApiService>();
            _kernel.Bind<ISerializationService>().To<JsonSerializationService>();
            _kernel.Bind<ISteganographyService>().To<SteganographyService>();
            _kernel.Bind<ICriptoService>().To<AesCriptoService>();
        }

        public static T Resolve<T>()
        {
            return _kernel.TryGet<T>();
        }
        public static object Resolve(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
        public static IEnumerable<object> ResolveAll(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}
