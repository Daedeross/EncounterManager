namespace EncounterManager.Services
{
    using System.Runtime.Serialization;
    using Foundation;
    using Microsoft.ServiceFabric.Actors;

    [DataContract]
    public class IndexState
    {
        [DataMember]
        public ActorReference Reference { get; set; }
        [DataMember]
        public long Index { get; set; }
        [DataMember]
        public Identity Identity { get; set; }
    }
}