namespace EncounterManager.Encounters
{
    using Foundation;
    using Foundation.ServiceFabric;
    using Foundation.Utilities;
    using Microsoft.ServiceFabric.Actors;
    using Microsoft.ServiceFabric.Actors.Runtime;

    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = "EncounterLoader")]
    public class EncounterLoader : CoreActorBase, IEncounterLoader
    {
        private IServiceFabricToolbox _toolbox;

        protected EncounterLoader(ActorService actorService, ActorId actorId, IServiceFabricToolbox toolbox)
            : base(actorService, actorId)
        {
            Args.NotNull(toolbox, nameof(toolbox));
            _toolbox = toolbox;
        }
    }
}
