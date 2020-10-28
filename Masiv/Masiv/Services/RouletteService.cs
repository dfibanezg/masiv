using Masiv.Entities.Business;
using Masiv.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Masiv.Services
{
    public class RouletteService : BaseBlobService<Roulette>, IRouletteService
    {
        public RouletteService(IConfiguration configuration) :
            base(connectionString: configuration.GetConnectionString("AzureStorageAccountConn"), containerName: configuration.GetConnectionString("RouletteContainerName"))
        { }

        public async Task<Guid> Add(string name)
        {
            Roulette roulette = new Roulette() { Id = Guid.NewGuid(), Name = name };
            await Add(roulette.Id.ToString(), roulette);

            return roulette.Id;
        }

        public new async Task<Roulette> Get(string rouletteId) => await base.Get(rouletteId);

        public async Task<Guid> Update(Roulette roulette)
        {
            await Add(roulette.Id.ToString(), roulette);
            return roulette.Id;
        }
    }
}
