namespace EncounterManager.Users
{
    using Castle.Windsor;
    using EncounterManager.Users.Configuration;
    using Foundation.ServiceFabric;
    using System;
    using System.Threading;

    public class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                using (var container = new WindsorContainer())
                {
                    container.Install(new ServiceInstaller());

                    // Prevents this host process from terminating so services keep running.
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e);
                throw;
            }
        }
    }
}
