using Learn_ASP.NET.Data;
using Learn_ASP.NET.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Learn_ASP.NET.IdentityStore
{
    public class UserStore : IUserPasswordStore<User, int>, IUserRoleStore<User, int>
    {
        private readonly ApplicationDbContext _context;
        public UserStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }


        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
        public Task<IList<string>> GetRolesAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            _context.Users.Attach(user);
            return _context.SaveChangesAsync();
        }
    }
}