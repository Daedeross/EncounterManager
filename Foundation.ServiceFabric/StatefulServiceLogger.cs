namespace Foundation.ServiceFabric
{
    using System;
    using Microsoft.ServiceFabric.Services.Runtime;

    public class StatefulServiceLogger : IServiceLogger
    {
        private readonly StatefulServiceBase _service;

        public StatefulServiceLogger(StatefulServiceBase service)
        {
            _service = service;
        }

        public void Message(string message, params object[] args)
        {
            ServiceEventSource.Current.ServiceMessage(_service, message, args);
        }

        public void Error(string message, params object[] args)
        {
            ServiceEventSource.Current.ServiceError(_service, message, args);
        }

        public void Exception(Exception exception)
        {
            ServiceEventSource.Current.ServiceException(_service, exception);
        }
    }
}