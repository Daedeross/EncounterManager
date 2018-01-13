namespace EncounterManager.Users.Model
{
    using Foundation;
    using System.Runtime.Serialization;

    [DataContract(Name = "UpdateUser", Namespace = "http://schemas.rpgmanager.com/users")]
    public class UserRecord: RecordBase
    {
        [DataMember(IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true, EmitDefaultValue = true)]
        public string NormalizedName { get; set; }
        [DataMember(IsRequired = true, EmitDefaultValue = true)]
        public string Email { get; set; }
    }
}
