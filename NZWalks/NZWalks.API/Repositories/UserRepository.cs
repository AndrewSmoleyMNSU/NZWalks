using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public UserRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await nZWalksDBContext.Users
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower() && x.Password.ToLower() == password);

            if (user == null)
            {
                return null;
            }

            var userRoles = await nZWalksDBContext.UsersRoles.Where(x => x.UserId == user.Id).ToListAsync();

            if (userRoles.Any())
            {
                user.Roles = new List<string>();

                foreach (var userRole in userRoles)
                {
                    var role = await nZWalksDBContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);

                    if (role != null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }

            user.Password = null;
            return user;

        }
    }
}
