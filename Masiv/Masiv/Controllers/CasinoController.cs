using Masiv.Entities.Business;
using Masiv.Entities.DataTransfer;
using Masiv.Enumerations;
using Masiv.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasinoController : ControllerBase
    {
        private readonly IRouletteService _rouletteService;
        private readonly IUserService _userService;

        public CasinoController(IRouletteService rouletteService, IUserService userService)
        {
            _rouletteService = rouletteService;
            _userService = userService;
        }

        [HttpPost("newroulette")]
        public async Task<Guid> NewRoulette(string name) => await _rouletteService.Add(name);

        [HttpGet("openroulette/{id}")]
        public async Task<IActionResult> OpenRoulette(string id)
        {
            if (!IsValidId(id))
                return BadRequest();

            Roulette roulette = await _rouletteService.Get(id);

            if (roulette == default)
                return NotFound();

            roulette.IsOpen = true;
            roulette.Bets = new List<Bet>();
            await _rouletteService.Update(roulette);

            return Ok(new { sucess = true });
        }

        [HttpPost("newuser")]
        public async Task<Guid> NewUser([FromBody] User user) => await _userService.Add(user);

        [HttpPut("addbet/{rouletteId}")]
        public async Task<IActionResult> AddBet([FromHeader] string userId, string rouletteId, BetDto betDto)
        {
            if (!IsValidId(userId) || !IsValidId(rouletteId))
                return BadRequest();

            if (betDto.Number < 0 || betDto.Number > 36 || !IsValidColorNumber(red: betDto.Red, number: betDto.Number))
                return BadRequest();

            User user = await _userService.GetUser(userId);
            Roulette roulette = await _rouletteService.Get(rouletteId);

            if (user == default || roulette == default)
                return NotFound();

            if (!roulette.IsOpen)
                return ValidationProblem(MessagesEnum.RouletteClosed);

            if (betDto.Money > user.Money)
                return ValidationProblem(MessagesEnum.UserNoFunds);

            if (betDto.Money > ConfigsEnum.HighestBet)
                return ValidationProblem(MessagesEnum.HighestBet);

            user.Money -= betDto.Money;
            await _userService.Update(user);

            Bet bet = new Bet()
            {
                Id = Guid.NewGuid(),
                Red = betDto.Red,
                Number = betDto.Number,
                Money = betDto.Money,
                User = user,
            };

            roulette.Bets.Add(bet);
            await _rouletteService.Update(roulette);

            return Ok();
        }

        private bool IsValidId(string id)
        {
            try
            {
                Guid.Parse(id);
                return true;
            }
            catch (Exception) { return false; }
        }

        private bool IsValidColorNumber(bool red, sbyte? number)
        {
            if (number == null)
                return true;
            return (number % 2) == 0 ? red : !red;
        }

        [HttpPut("closeroulette/{id}")]
        public async Task<IActionResult> CloseRoulette(string id)
        {
            if (!IsValidId(id))
                return BadRequest();

            Roulette roulette = await _rouletteService.Get(id);

            if (roulette == default)
                return NotFound();

            roulette.IsOpen = false;
            await _rouletteService.Update(roulette);

            roulette.WinnerNumber = (sbyte)new Random().Next(0, 37);
            roulette.IsRedWinnerColor = (roulette.WinnerNumber % 2) == 0;

            foreach (Bet bet in roulette.Bets)
            {
                if (bet.Number == roulette.WinnerNumber)
                {
                    bet.Gain = bet.Money * 5;
                    bet.User.Money += bet.Gain;
                    await _userService.Update(bet.User);
                    continue;
                }

                if (bet.Red == roulette.IsRedWinnerColor)
                {
                    bet.Gain = (float)(bet.Money * 1.8);
                    bet.User.Money += bet.Gain;
                    await _userService.Update(bet.User);
                    continue;
                }
            }

            return Ok(roulette);
        }
    }
}
