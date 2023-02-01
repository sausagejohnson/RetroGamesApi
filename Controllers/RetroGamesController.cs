using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RetroGamesApi.Controllers
{
    [ApiController]
    [Route("api")]
    [CreateDataActionFilterAttribute]
    public class RetroGamesController : ControllerBase
    {

        [HttpGet]
        [Route("games")]
        public IEnumerable<Game> Get()
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext);
            Games games = sentinel.ConvertRawToGames();

            return games.GamesList;
        }

        [HttpGet]
        [Route("games/{id}")]
        public Game Get(int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext);
            Games games = sentinel.ConvertRawToGames();
            Game game = games.GamesList.FirstOrDefault(g => g.GameId == id);

            return game;
        }

        [HttpPost]
        [Route("games")]
        public IEnumerable<Game> Post([FromBody] Game game)
        {
            game.GameId = new Random().Next(11, 999999);

            DataSentinel sentinel = new DataSentinel(this.HttpContext);
            Games games = sentinel.ConvertRawToGames();
            games.GamesList.Add(game);
            sentinel.WriteNewGameFile(games);

            return games.GamesList;
        }
    }
}
