namespace EncounterManager.Users
{
    using EncounterManager.Users.Model;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.ServiceFabric.Actors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUserLoader: IActor
    {
        Task<IdentityResult> CreateUserAsync(UserRecord user, CancellationToken cancellationToken);

        Task<IdentityResult> DeleteUserAsync(UserRecord user, CancellationToken cancellationToken);

        Task<IdentityResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken);
    }
}
