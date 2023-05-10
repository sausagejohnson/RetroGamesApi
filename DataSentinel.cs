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

        public List<GameFullDTO> GetAllGames()
        {
            List<GameFullDTO> gamesDtoList = this.ConvertJSONToFullGamesDTO();

            return gamesDtoList;
        }

        public GameFullDTO GetGameById(int id)
        {
            List<GameFullDTO> games = this.GetAllGames();
            GameFullDTO game = games.FirstOrDefault(g => g.GameId == id);

            return game;
        }

        private List<GameFullDTO> ConvertJSONToFullGamesDTO()
        {
            CreateDataFilesIfNotExists();
            string jsonContent = File.ReadAllText(this.GameFilePath);
            Games games = JsonConvert.DeserializeObject<Games>(jsonContent);

            List<GameFullDTO> gameDtoList = GamesToFullDTO(games);

            return gameDtoList;
        }

        private List<GameFullDTO> GamesToFullDTO(Games games)
        {
            List<GameFullDTO> gamesDtoList = new List<GameFullDTO>();
            games.GamesList.ForEach(g =>
            {
                gamesDtoList.Add(new GameFullDTO
                {
                    GameId = g.GameId,
                    Platforms = GetPlatformsByPlatformId(g.platformIds),
                    Title = g.Title,
                    Year = g.Year
                }
                );
            });

            return gamesDtoList;
        }

        private Games FullDtosToGames(List<GameFullDTO> gamesDTOList)
        {
            Games games = new Games();

            gamesDTOList.ForEach(dto => {
                int[] platformIds = dto.Platforms.Select(p => { return p.PlatformId; } ).ToArray();

                games.GamesList.Add(new Game() { 
                    GameId = dto.GameId,
                    Title = dto.Title,
                    Year = dto.Year,
                    platformIds = platformIds
                });
            });

            return games;
        }

        private Game FullDtoToGame(GameFullDTO fullGameDTO)
        {
            Game game = new Game {
                GameId = fullGameDTO.GameId,
                Title = fullGameDTO.Title,
                Year = fullGameDTO.Year,
                platformIds = fullGameDTO.Platforms.Select(p => { return p.PlatformId; }).ToArray()
            };

            return game;
        }

        private GameDTO FullDtoToGameDto(GameFullDTO fullGameDTO)
        {
            GameDTO game = new GameDTO
            {
                GameId = fullGameDTO.GameId,
                Title = fullGameDTO.Title,
                Year = fullGameDTO.Year,
                platformIds = fullGameDTO.Platforms.Select(p => { return p.PlatformId; }).ToArray()
            };

            return game;
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

        public List<GameFullDTO> AddPlatformIdToGameId(int id, int platformId)
        {
            List<GameFullDTO> fullDtoGames = this.ConvertJSONToFullGamesDTO();
            GameFullDTO dtoGame = fullDtoGames.FirstOrDefault(g => g.GameId == id);

            if (dtoGame == null)
            {
                return null;
            }

            Platform newPlatform = this.GetPlatformByPlatformId(platformId);
            dtoGame.Platforms.Add(newPlatform);

            GameDTO game = this.FullDtoToGameDto(dtoGame);
            
            fullDtoGames = UpdateGame(game, game.GameId);
            return fullDtoGames;

        }

        public List<GameFullDTO> DeletePlatformIdFromGameId(int id, int platformId)
        {
            List<GameFullDTO> fullDtoGames = this.ConvertJSONToFullGamesDTO();
            GameFullDTO dtoGame = fullDtoGames.FirstOrDefault(g => g.GameId == id);
            GameDTO game = this.FullDtoToGameDto(dtoGame);

            if (game == null)
            {
                return null;
            }

            if (game.platformIds.Contains(platformId))
            {
                List<Platform> newPlatforms = dtoGame.Platforms;
                newPlatforms.Remove(this.GetPlatformByPlatformId(platformId));

                game.platformIds = newPlatforms.Select(p => p.PlatformId).ToArray();
            }

            List<GameFullDTO> updatedGames = UpdateGame(game, game.GameId);
            return updatedGames; 

        }

        public List<Platform> GetPlatformsByGameId(int id)
        {
            List<GameFullDTO> games = this.ConvertJSONToFullGamesDTO();
            GameFullDTO game = games.FirstOrDefault(g => g.GameId == id);
            if (game == null)
            {
                return null;
            }

            //List<Platform> platforms = game.Platforms;//this.ConvertJSONToPlatforms();
            //List<Platform> gamePlatforms = platforms.PlatformsList.FindAll(p => game.platformIds.Contains(p.PlatformId));

            return game.Platforms;
        }

        public Platform GetPlatformByPlatformId(int id)
        {
            Platforms platforms = this.ConvertJSONToPlatforms();
            Platform gamePlatform = platforms.PlatformsList.FirstOrDefault(p => p.PlatformId == id);

            return gamePlatform;
        }

        public List<Platform> GetPlatformsByPlatformId(int[] ids)
        {
            Platforms platforms = this.ConvertJSONToPlatforms();
            List<Platform> gamePlatforms = platforms.PlatformsList.FindAll(p => ids.Contains(p.PlatformId));

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

        public List<GameFullDTO> AddGame(GameDTO newGame)
        {
            newGame.GameId = this.GetNextID().Value;
            
            if (newGame.platformIds == null)
            {
                newGame.platformIds = new int[0];
            }

            List<GameFullDTO> dtoGames = GetAllGames();
            Games games = FullDtosToGames(dtoGames);

            games.GamesList.Add(new Game { 
                GameId = newGame.GameId,
                Title = newGame.Title, 
                Year = newGame.Year,
                platformIds = newGame.platformIds
            });

            this.WriteNewGameFile(games);

            return dtoGames;
        }

        public List<GameFullDTO> UpdateGame(GameDTO updatedGame, int gameId)
        {
            if (gameId == 0)
                return null;

            List<GameFullDTO> dtoGames = this.ConvertJSONToFullGamesDTO();

            if (updatedGame == null)
            {
                return null;
            }

            Games games = this.FullDtosToGames(dtoGames);
            Game game = games.GamesList.Find(x => x.GameId == gameId);
            GameFullDTO fullDtoGame = dtoGames.Find(x => x.GameId == gameId);


            if (updatedGame.Title != null) { 
                game.Title = updatedGame.Title;
                fullDtoGame.Title = updatedGame.Title; 
            }
            if (updatedGame.Year > 0)
            {
                game.Year = updatedGame.Year;
                fullDtoGame.Year = updatedGame.Year;
            }
            if (updatedGame.platformIds != null)
            {
                game.platformIds = updatedGame.platformIds;
                fullDtoGame.Platforms = this.GetPlatformsByPlatformId(updatedGame.platformIds);
            }

            this.WriteNewGameFile(games);

            return dtoGames;
        }

        public List<GameFullDTO> DeleteGame(int gameIdToDelete)
        {
            List<GameFullDTO> dtoGames = this.ConvertJSONToFullGamesDTO();
            GameFullDTO game = dtoGames.Find(x => x.GameId == gameIdToDelete);

            if (game != null)
            {
                dtoGames.Remove(game);

                Games games = this.FullDtosToGames(dtoGames);
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
            List<GameFullDTO> games = this.ConvertJSONToFullGamesDTO();
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
