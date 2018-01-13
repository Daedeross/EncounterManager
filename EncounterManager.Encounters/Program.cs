namespace EncounterManager.Encounters
{
    using Castle.Windsor;
    using EncounterManager.Encounters.Configuration;
    using Foundation.ServiceFabric;
    using System;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
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
            catch (Exception exception)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(exception);
                throw;
            }
        }
    }
}
