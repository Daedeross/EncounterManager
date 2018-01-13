// explicitly use the same namespace as all the other IServiceProxyFactory extension methods
// ReSharper disable once CheckNamespace 
namespace Microsoft.ServiceFabric.Services.Remoting.Client
{
    using System;
    using System.Fabric;
    using Foundation;
    using Microsoft.ServiceFabric.Services.Client;
    using Microsoft.ServiceFabric.Services.Remoting;

    public static class ServiceProxyFactoryExtensions
    {
        public static TServiceInterface CreateServiceProxy<TServiceInterface>(this IServiceProxyFactory actorProxyFactory, ServiceReference serviceReference)
            where TServiceInterface : IService
        {
            if (serviceReference == null)
            {
                throw new ArgumentNullException(nameof(serviceReference));
            }

            return actorProxyFactory.CreateServiceProxy<TServiceInterface>(serviceReference.ServiceUri, GetPartitionKey(serviceReference));
        }

        internal static ServicePartitionKey GetPartitionKey(ServiceReference serviceReference)
        {
            switch (serviceReference.PartitionKind)
            {
                case ServicePartitionKind.Invalid:
                    return null;
                case ServicePartitionKind.Singleton:
                    return new ServicePartitionKey();
                case ServicePartitionKind.Int64Range:
                    if (!serviceReference.PartitionId.HasValue)
                    {
                        throw new ArgumentException("Missing a valid PartitionId for ServicePartitionKind.Int64Range");
                    }
                    return new ServicePartitionKey(serviceReference.PartitionId.Value);
                case ServicePartitionKind.Named:
                    if (string.IsNullOrEmpty(serviceReference.PartitionName))
                    {
                        throw new ArgumentException("Missing a valid Name for ServicePartitionKind.Named");
                    }
                    return new ServicePartitionKey(serviceReference.PartitionName);
                default:
                    return null;
            }

        }
    }
}