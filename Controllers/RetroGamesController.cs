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
    public class RetroGamesController : ControllerBase
    {

        [HttpGet]
        [Route("games")]
        public IEnumerable<Game> Get()
        {
            string jsonContent = System.IO.File.ReadAllText("games.json");
            Games games = JsonConvert.DeserializeObject<Games>(jsonContent);

            return games.GamesList;
        }

        [HttpGet]
        [Route("games/{id}")]
        public Game Get(int id)
        {
            string jsonContent = System.IO.File.ReadAllText("games.json");
            Games games = JsonConvert.DeserializeObject<Games>(jsonContent);
            Game game = games.GamesList.FirstOrDefault(g => g.GameId == id);

            return game;
        }
    }
}
