using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zeus.Enums.Users;
using Zeus.Models.Users.Queries;
using Zeus.Utilities.Extensions;

namespace Zeus.Infrastructure.Handlers.Users.Queries
{
   internal sealed class GetUserRoleDictionaryHandler : IRequestHandler<GetUserRoleDictionaryQuery, IEnumerable<KeyValuePair<int, string>>>
   {
      public Task<IEnumerable<KeyValuePair<int, string>>> Handle(GetUserRoleDictionaryQuery request, CancellationToken cancellationToken)
      {
         return Task.FromResult(EnumExtensions.GetValues<UserRole>());
      }
   }
}
