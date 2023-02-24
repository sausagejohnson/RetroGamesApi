using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace RetroGamesApi.Controllers
{
    [ApiController]
    [Route("api")]
    [CreateDataActionFilterAttribute]
    public class RetroGamesController : ControllerBase
    {

        [HttpGet]
        [Route("games")]
        public IActionResult Get()
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            Games games = sentinel.GetAllGames();

            return Ok(games.GamesList);
        }

        [HttpGet]
        [Route("games/{id}")]
        public IActionResult Get(int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            Game game = sentinel.GetGameById(id);

            return Ok(game);
        }

        [HttpPost]
        [Route("games")]
        public IActionResult Post([FromBody] Game game)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            Games updatedGames = sentinel.AddGame(game);

            return Created("/games", updatedGames.GamesList);
        }

        [HttpPut]
        [Route("games")]
        public IActionResult Put([FromBody] Game game)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            Games updatedGames = sentinel.UpdateGame(game);

            if (updatedGames == null)
            {
                return NotFound();
            }

            return Ok(updatedGames.GamesList);
        }

        [HttpDelete]
        [Route("games/{id}")]
        public IActionResult Delete(int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            Games updatedGames = sentinel.DeleteGame(id);
            if (updatedGames != null && updatedGames.GamesList.Any())
                return Ok(updatedGames.GamesList);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("games/{id}/platforms")]
        public IActionResult GetPlatforms(int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            List<Platform> platforms = sentinel.GetPlatformsByGameId(id);

            return Ok(platforms);
        }
        
        [HttpGet]
        [Route("platforms")]
        public IActionResult GetAllPlatforms()
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            Platforms platforms = sentinel.GetAllPlatforms();

            return Ok(platforms.PlatformsList);
        }
    }
}
