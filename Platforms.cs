using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroGamesApi
{
    public class Platforms
    {
        [JsonProperty("platforms")]
        public List<Platform> PlatformsList { get; set; }
    }
}