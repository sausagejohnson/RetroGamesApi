using Microsoft.AspNetCore.Mvc;
using RetroGamesApi.DTOs;
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
            List<GameDTO> games = sentinel.GetAllGames();

            return Ok(games);
        }

        [HttpGet]
        [Route("games/{id}")]
        public IActionResult Get(int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            GameDTO game = sentinel.GetGameById(id);

            return Ok(game);
        }

        [HttpPost]
        [Route("games")]
        public IActionResult Post([FromBody] GameDTO game)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            List<GameDTO> updatedGames = sentinel.AddGame(game);

            return Created("/games", updatedGames);
        }

        [HttpPut]
        [Route("games/{id}")]
        public IActionResult Put([FromBody] GameDTO game, int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            List<GameDTO> updatedGames = sentinel.UpdateGame(game, id);

            if (updatedGames == null)
            {
                return NotFound();
            }

            return Ok(updatedGames);
        }

        [HttpDelete]
        [Route("games/{id}")]
        public IActionResult Delete(int id)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            List<GameDTO> updatedGames = sentinel.DeleteGame(id);
            if (updatedGames != null && updatedGames.Any())
                return Ok(updatedGames);
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

        [HttpPost]
        [Route("games/{id}/platform/{platformId}")]
        public IActionResult AddPlatform(int id, int platformId)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            List<GameDTO> games = sentinel.AddPlatformIdToGameId(id, platformId);

            return Ok(games);
        }

        [HttpDelete]
        [Route("games/{id}/platform/{platformId}")]
        public IActionResult DeletePlatform(int id, int platformId)
        {
            DataSentinel sentinel = new DataSentinel(this.HttpContext.Connection.Id);
            List<GameDTO> games = sentinel.DeletePlatformIdFromGameId(id, platformId);

            return Ok(games);
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
