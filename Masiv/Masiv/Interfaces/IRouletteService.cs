using Masiv.Entities.Business;
using System;
using System.Threading.Tasks;

namespace Masiv.Interfaces
{
    public interface IRouletteService
    {
        Task<Guid> Add(string name);
        Task<Roulette> Get(string rouletteId);
        Task<Guid> Update(Roulette roulette);
    }
}