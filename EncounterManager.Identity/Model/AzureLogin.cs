using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterManager.Identity.Model
{
    public class AzureLogin : TableEntity
    {
        public AzureLogin()
        {
            PartitionKey = IdentityConstants.IdentityPartitionKey;
            RowKey = Guid.NewGuid().ToString();
        }

        public AzureLogin(string ownerId, UserLoginInfo info) : this()
        {
            UserId = ownerId;
            LoginProvider = info.LoginProvider;
            ProviderKey = info.ProviderKey;
        }

        public string UserId { get; set; }
        public string ProviderKey { get; set; }
        public string LoginProvider { get; set; }
    }
}
