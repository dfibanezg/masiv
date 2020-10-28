using Masiv.Entities.Business;
using Masiv.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Masiv.Services
{
    public class UserService : BaseBlobService<User>, IUserService
    {
        public UserService(IConfiguration configuration) :
            base(connectionString: configuration.GetConnectionString("AzureStorageAccountConn"), containerName: configuration.GetConnectionString("UserContainerName"))
        { }

        public async Task<User> GetUser(string id) => await Get(id);

        public async Task<Guid> Add(User user)
        {
            user.Id = Guid.NewGuid();
            await Add(user.Id.ToString(), user);
            return user.Id;
        }

        public async Task Update(User user) => await Add(user.Id.ToString(), user);
    }
}
