namespace EncounterManager.Users.Model
{
    using Foundation;
    using System.Runtime.Serialization;
    
    [DataContract(Name = "UpdateUser", Namespace = "http://schemas.rpgmanager.com/users")]
    public class UpdateUserRequest: RequestBase
    {
        [DataMember(IsRequired = true, EmitDefaultValue = true)]
        public Identity Id { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string NormalizedName { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string PasswordHash { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Email { get; set; }
    }
}
