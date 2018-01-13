namespace Foundation.ServiceFabric
{
    using System;

    public interface IServiceLogger
    {
        void Message(string message, params object[] args);
        void Error(string message, params object[] args);
        void Exception(Exception exception);
    }
}