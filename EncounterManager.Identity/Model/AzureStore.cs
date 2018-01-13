using EncounterManager.Storage;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EncounterManager.Identity.Model
{
    public class AzureStore : IUserStore<AzureUser>, IUserClaimStore<AzureUser>, IUserLoginStore<AzureUser>, IUserRoleStore<AzureUser>, IUserPasswordStore<AzureUser>
    {
        public AzureStore()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;

            // CreateAsync the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // CreateAsync the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("Identity");
            table.CreateIfNotExists();
            Table = table;

            BatchOperation = new TableBatchOperation();
        }

        public TableBatchOperation BatchOperation { get; protected set; }
        public CloudTable Table { get; protected set; }
        
        public void Dispose()
        {
        }

        #region IUserStore

        public Task CreateAsync(AzureUser user)
        {
            var op = TableOperation.Insert(user);
            return Table.ExecuteAsync(op);
        }

        public Task DeleteAsync(AzureUser user)
        {
            var op = TableOperation.Delete(user);
            return Table.ExecuteAsync(op);
        }

        public async Task<AzureUser> FindByIdAsync(string userId)
        {
            var op = TableOperation.Retrieve<AzureUser>(IdentityConstants.IdentityPartitionKey, userId);
            var result = await Table.ExecuteAsync(op);
            return result.Result as AzureUser;
        }

        public async Task<AzureUser> FindByNameAsync(string userName)
        {
            var query = new TableQuery<AzureUser>()
                .Where(TableQuery.GenerateFilterCondition("UserName", QueryComparisons.Equal, userName));
            var result = await Table.ExecuteQueryAsync<AzureUser>(query);
            return result.FirstOrDefault();
        }

        public Task UpdateAsync(AzureUser user)
        {
            var op = TableOperation.Replace(user);
            return Table.ExecuteAsync(op);
        }

        #endregion // IUserStore

        #region IUserClaimStore

        public Task<IList<Claim>> GetClaimsAsync(AzureUser user)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimAsync(AzureUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(AzureUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        #endregion // IUserClaimStore
        
        #region IUserLoginStore
        public Task AddLoginAsync(AzureUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(AzureUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AzureUser user)
        {
            throw new NotImplementedException();
        }

        public Task<AzureUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        #endregion // IUserLoginStore

        #region IUserRoleStore
        public Task AddToRoleAsync(AzureUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(AzureUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(AzureUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(AzureUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion // IUserRoleStore

        #region IUserPasswordStore
        
        public Task SetPasswordHashAsync(AzureUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(AzureUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(AzureUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
