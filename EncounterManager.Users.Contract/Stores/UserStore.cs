namespace EncounterManager.Users.Stores
{
    using EncounterManager.Users.Model;
    using Foundation;
    using Foundation.Utilities;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserStore : IUserStore<UserRecord>, IUserPasswordStore<UserRecord>
    {
        private IServiceFabricToolbox _toolbox;

        public UserStore(IServiceFabricToolbox toolbox)
        {
            Args.NotNull(toolbox, nameof(toolbox));
            _toolbox = toolbox;
        }

        public void Dispose()
        {

        }

        #region IUSerStore

        public Task<IdentityResult> CreateAsync(UserRecord user, CancellationToken cancellationToken)
        {
            var loader = _toolbox.UserLoader();
            return loader.CreateUserAsync(user, cancellationToken);
        }

        public Task<IdentityResult> DeleteAsync(UserRecord user, CancellationToken cancellationToken)
        {
            var loader = _toolbox.UserLoader();
            return loader.DeleteUserAsync(user, cancellationToken);
        }


        public async Task<UserRecord> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var registry = _toolbox.UserRegistry();
            var userReference = await registry.FindUserByIdentityAsync((Identity)userId, cancellationToken);

            if (userReference == null)
            {
                return null;
            }

            var user = _toolbox.User(userReference);
            return await user.GetRecordAsync();
        }

        public async Task<UserRecord> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var registry = _toolbox.UserRegistry();
            var userReference = await registry.FindUserByNormalizedNameAsync(normalizedUserName, cancellationToken);

            if (userReference == null)
            {
                return null;
            }

            var user = _toolbox.User(userReference);
            return await user.GetRecordAsync();
        }

        public async Task<string> GetNormalizedUserNameAsync(UserRecord user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(user.NormalizedName))
            {
                return user.NormalizedName;
            }

            var userId = user.Id;
            var registry = _toolbox.UserRegistry();
            var userReference = await registry.FindUserByIdentityAsync(userId, cancellationToken);

            if (userReference == null)
            {
                return null;
            }

            var userActor = _toolbox.User(userReference);
            return await userActor.GetNormalizedNameAsync();
        }

        public async Task<string> GetUserIdAsync(UserRecord user, CancellationToken cancellationToken)
        {
            if (!Equals(user.Id, Identity.None))
            {
                return user.Id.GetStringId();
            }

            var userId = user.Id;
            var registry = _toolbox.UserRegistry();
            var userReference = await registry.FindUserByIdentityAsync(userId, cancellationToken);

            if (userReference == null)
            {
                return null;
            }

            var userActor = _toolbox.User(userReference);
            return await userActor.GetIdentityAsync().Then(id => id.GetStringId());
        }

        public async Task<string> GetUserNameAsync(UserRecord user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                return user.Name;
            }

            var userId = user.Id;
            var registry = _toolbox.UserRegistry();
            var userReference = await registry.FindUserByIdentityAsync(userId, cancellationToken);

            if (userReference == null)
            {
                return null;
            }

            var userActor = _toolbox.User(userReference);
            return await userActor.GetNameAsync();
        }

        public Task SetNormalizedUserNameAsync(UserRecord user, string normalizedName, CancellationToken cancellationToken)
        {
            var loader = _toolbox.UserLoader();
            return loader.UpdateUserAsync(new UpdateUserRequest
            {
                Id = user.Id,
                NormalizedName = normalizedName,
            }, cancellationToken);
        }

        public Task SetUserNameAsync(UserRecord user, string userName, CancellationToken cancellationToken)
        {
            var loader = _toolbox.UserLoader();
            return loader.UpdateUserAsync(new UpdateUserRequest
            {
                Id = user.Id,
                Name = userName,
            }, cancellationToken);
        }

        public Task<IdentityResult> UpdateAsync(UserRecord user, CancellationToken cancellationToken)
        {
            var loader = _toolbox.UserLoader();
            return loader.UpdateUserAsync(new UpdateUserRequest
            {
                Id = user.Id,
                Name = user.Name,
                NormalizedName = user.NormalizedName,
                Email = user.Email
            }, cancellationToken);
        }

        #endregion

        #region IUserPasswordStore

        public Task SetPasswordHashAsync(UserRecord user, string passwordHash, CancellationToken cancellationToken)
        {
            var loader = _toolbox.UserLoader();
            return loader.UpdateUserAsync(new UpdateUserRequest
            {
                Id = user.Id,
                PasswordHash = passwordHash
            }, cancellationToken);
        }

        public async Task<string> GetPasswordHashAsync(UserRecord user, CancellationToken cancellationToken)
        {
            if (Equals(user.Id, Identity.None))
            {
                throw new ArgumentException("User must have an Id");
            }

            var registry = _toolbox.UserRegistry();

            var userId = user.Id;
            var userReference = await registry.FindUserByIdentityAsync(userId, cancellationToken);

            if (userReference == null)
            {
                return null;
            }

            var userActor = _toolbox.User(userReference);
            return await userActor.GetPasswordHashAsync();
        }

        public Task<bool> HasPasswordAsync(UserRecord user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion // IUserPasswordStore
    }
}
