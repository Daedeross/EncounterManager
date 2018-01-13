using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EncounterManager.Identity.Model
{
    public class AzureUser : TableEntity, IUser
    {
        public AzureUser()
        {
            PartitionKey = IdentityConstants.IdentityPartitionKey;
            RowKey = Guid.NewGuid().ToString();
            Id = RowKey;
            Roles = new List<string>();
            Claims = new List<Claim>();
            Logins = new List<AzureLogin>();
        }

        public AzureUser(string userName) : this()
        {
            UserName = userName;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public IList<string> Roles { get; set; }
        public IList<AzureLogin> Logins { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
