using Microsoft.AspNetCore.Mvc;
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
            Games games = sentinel.ConvertJSONToGames();

            return Ok(games.GamesList);
        }

        [HttpGet]
        [Route("games/{id}")]
        public IActionResult Get(int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            Games games = sentinel.ConvertJSONToGames();
            Game game = games.GamesList.FirstOrDefault(g => g.GameId == id);

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
    }
}
