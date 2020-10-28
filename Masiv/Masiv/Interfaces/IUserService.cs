using Masiv.Entities.Business;
using System;
using System.Threading.Tasks;

namespace Masiv.Interfaces
{
    public interface IUserService
    {
        Task<Guid> Add(User user);
        Task<User> GetUser(string id);
        Task Update(User user);
    }
}
