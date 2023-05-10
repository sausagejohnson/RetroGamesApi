using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroGamesApi.DTOs
{
    //DTO used for display for a consuming client
    public class GameFullDTO
    {
        public int GameId { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public List<Platform> Platforms { get; set; }

    }

}
