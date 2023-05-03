using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using RetroGamesApi.DTOs;

namespace RetroGamesApi
{
    public class DataSentinel
    {
        public string PublicId { get; set; }
        public string Folder { get; set; }
        public string GameFilePath { get; set; }
        public string PlatformsFilePath { get; set; }

        public DataSentinel(string connectionId)
        {
            PublicId = connectionId;
            Folder = @"publicdata\";
            GameFilePath = Folder + PublicId + ".json";
            PlatformsFilePath = Folder + PublicId + "-platform.json";

            CreateDataFolderIfNotExists();
            CleanUpDataFiles();
        }

        public List<GameDTO> GetAllGames()
        {
            List<GameDTO> gamesDtoList = this.ConvertJSONToGames();

            return gamesDtoList;
        }

        public GameDTO GetGameById(int id)
        {
            List<GameDTO> games = this.GetAllGames();
            GameDTO game = games.FirstOrDefault(g => g.GameId == id);

            return game;
        }

        private List<GameDTO> ConvertJSONToGames()
        {
            CreateDataFilesIfNotExists();
            string jsonContent = File.ReadAllText(this.GameFilePath);
            Games games = JsonConvert.DeserializeObject<Games>(jsonContent);

            List<GameDTO> gameDtoList = GamesToDTO(games);

            return gameDtoList;
        }

        private List<GameDTO> GamesToDTO(Games games)
        {
            List<GameDTO> gamesDtoList = new List<GameDTO>();
            games.GamesList.ForEach(g =>
            {
                gamesDtoList.Add(new GameDTO
                {
                    GameId = g.GameId,
                    platformIds = g.platformIds,
                    Title = g.Title,
                    Year = g.Year
                }
                );
            });

            return gamesDtoList;
        }

        private Games DTOToGames(List<GameDTO> gamesList)
        {
            Games games = new Games();
            games.GamesList = gamesList;

            return games;
        }

        public Platforms GetAllPlatforms()
        {
            return this.ConvertJSONToPlatforms();
        }

        private Platforms ConvertJSONToPlatforms()
        {
            CreateDataFilesIfNotExists();
            string jsonContent = File.ReadAllText(this.PlatformsFilePath);
            Platforms platforms = JsonConvert.DeserializeObject<Platforms>(jsonContent);

            return platforms;
        }

        public List<GameDTO> AddPlatformIdToGameId(int id, int platformId)
        {
            List<GameDTO> games = this.ConvertJSONToGames();
            GameDTO game = games.FirstOrDefault(g => g.GameId == id);

            if (game == null)
            {
                return null;
            }

            if (!game.platformIds.Contains(platformId))
            {
                List<int> platformIdList = game.platformIds.ToList();
                platformIdList.Add(platformId);

                game.platformIds = platformIdList.ToArray();
            }

            games = UpdateGame(game, game.GameId);
            return games;

        }

        public List<GameDTO> DeletePlatformIdFromGameId(int id, int platformId)
        {
            List<GameDTO> games = this.ConvertJSONToGames();
            GameDTO game = games.FirstOrDefault(g => g.GameId == id);

            if (game == null)
            {
                return null;
            }

            if (game.platformIds.Contains(platformId))
            {
                List<int> platformIdList = game.platformIds.ToList();
                platformIdList.Remove(platformId);

                game.platformIds = platformIdList.ToArray();
            }

            games = UpdateGame(game, game.GameId);
            return games;

        }

        public List<Platform> GetPlatformsByGameId(int id)
        {
            List<GameDTO> games = this.ConvertJSONToGames();
            GameDTO game = games.FirstOrDefault(g => g.GameId == id);
            if (game == null)
            {
                return null;
            }

            Platforms platforms = this.ConvertJSONToPlatforms();
            List<Platform> gamePlatforms = platforms.PlatformsList.FindAll(p => game.platformIds.Contains(p.PlatformId));

            return gamePlatforms;
        }

        public void CreateDataFilesIfNotExists()
        {
            string expectedDataFilePath = this.GameFilePath;
            string expectedPlatformDataFilePath = this.PlatformsFilePath;

            if (!File.Exists(expectedDataFilePath))
            {
                File.Copy("games.json", expectedDataFilePath);
            }

            if (!File.Exists(expectedPlatformDataFilePath))
            {
                File.Copy("platforms.json", expectedPlatformDataFilePath);
            }
        }

        public List<GameDTO> AddGame(GameDTO newGame)
        {
            newGame.GameId = this.GetNextID().Value;
            
            if (newGame.platformIds == null)
            {
                newGame.platformIds = new int[0];
            }

            List<GameDTO> dtoGames = this.ConvertJSONToGames();
            dtoGames.Add(newGame);

            Games games = DTOToGames(dtoGames);
            this.WriteNewGameFile(games);

            return dtoGames;
        }

        public List<GameDTO> UpdateGame(GameDTO updatedGame, int gameId)
        {
            if (gameId == 0)
                return null;

            List<GameDTO> dtoGames = this.ConvertJSONToGames();
            GameDTO dtoGame = dtoGames.Find(x => x.GameId == gameId);

            if (dtoGame == null)
            {
                return null;
            } 

            if (updatedGame.Title != null)
                dtoGame.Title = updatedGame.Title;
            if (updatedGame.Year > 0)
                dtoGame.Year = updatedGame.Year;
            if (updatedGame.platformIds != null) 
                dtoGame.platformIds = updatedGame.platformIds; 

            Games games = DTOToGames(dtoGames);
            this.WriteNewGameFile(games);

            return dtoGames;
        }

        public List<GameDTO> DeleteGame(int gameIdToDelete)
        {
            List<GameDTO> dtoGames = this.ConvertJSONToGames();
            GameDTO game = dtoGames.Find(x => x.GameId == gameIdToDelete);

            if (game != null)
            {
                dtoGames.Remove(game);

                Games games = this.DTOToGames(dtoGames);
                this.WriteNewGameFile(games);

                return dtoGames;
            }
            return null;
        }

        internal void WriteNewGameFile(Games games)
        {
            string json = JsonConvert.SerializeObject(games, Formatting.Indented);
            File.WriteAllText(this.GameFilePath, json);
        }

        private int? GetNextID()
        {
            int? id = null;
            List<GameDTO> games = this.ConvertJSONToGames();
            games.ForEach(game => 
                id = (game.GameId > id || id == null) ? game.GameId : id
            );

            return ++id;
        }

        private void CreateDataFolderIfNotExists()
        {
            if (!Directory.Exists(this.Folder))
            {
                Directory.CreateDirectory(this.Folder);
            }
        }

        private void CleanUpDataFiles()
        {
            CreateDataFolderIfNotExists();

            string[] userDataFiles = Directory.GetFiles(this.Folder);
            foreach (string file in userDataFiles)
            {
                DateTime createdTime = File.GetCreationTimeUtc(file);
                DateTime nowUTC = DateTime.UtcNow;

                TimeSpan difference = nowUTC.Subtract(createdTime);
                if (difference.TotalSeconds > 300)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}
