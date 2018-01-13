namespace Foundation
{
    using System;
    using System.Fabric;
    using System.Reflection;
    using Microsoft.ServiceFabric.Services.Runtime;

    public static class ServiceExtensions
    {
        /// <summary>
        /// Gets a reference to a StatefulService.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns><see cref="ServiceReference"/></returns>
        public static ServiceReference GetServiceReference(this StatefulServiceBase service)
        {
            return CreateServiceReference(service.Context, GetServicePartition(service).PartitionInfo);
        }

        /// <summary>
        /// Gets a reference to a StatelessService.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns><see cref="ServiceReference"/></returns>
        public static ServiceReference GetServiceReference(this StatelessService service)
        {
            return CreateServiceReference(service.Context, GetServicePartition(service).PartitionInfo);
        }


        /// <summary>
        /// Creates a <see cref="ServiceReference"/> for the provided service context and partition info.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static ServiceReference CreateServiceReference(ServiceContext context, ServicePartitionInformation info)
        {
            var serviceReference = new ServiceReference
            {
                ApplicationName = context.CodePackageActivationContext.ApplicationName,
                PartitionKind = info.Kind,
                ServiceUri = context.ServiceName,
                PartitionGuid = context.PartitionId,
            };

            var longInfo = info as Int64RangePartitionInformation;

            if (longInfo != null)
            {
                serviceReference.PartitionId = longInfo.LowKey;
            }
            else
            {
                var stringInfo = info as NamedPartitionInformation;
                if (stringInfo != null)
                {
                    serviceReference.PartitionName = stringInfo.Name;
                }
            }
            return serviceReference;
        }

        /// <summary>
        /// Gets the Partition info for the provided StatefulServiceBase instance.
        /// </summary>
        /// <param name="serviceBase"></param>
        /// <returns></returns>
        private static IStatefulServicePartition GetServicePartition(this StatefulServiceBase serviceBase)
        {
            if (serviceBase == null) throw new ArgumentNullException(nameof(serviceBase));
            return (IStatefulServicePartition)serviceBase
                .GetType()
                .GetProperty("Partition", BindingFlags.Instance | BindingFlags.NonPublic)?
                .GetValue(serviceBase);
        }

        /// <summary>
        /// Gets the Partition info for the provided StatelessService instance.
        /// </summary>
        /// <param name="serviceBase"></param>
        /// <returns></returns>
        private static IStatelessServicePartition GetServicePartition(this StatelessService serviceBase)
        {
            if (serviceBase == null) throw new ArgumentNullException(nameof(serviceBase));
            return (IStatelessServicePartition)serviceBase
                .GetType()
                .GetProperty("Partition", BindingFlags.Instance | BindingFlags.NonPublic)?
                .GetValue(serviceBase);
        }

    }
}