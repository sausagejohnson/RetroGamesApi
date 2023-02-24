using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RetroGamesApi
{
    public class DataSentinel
    {
        public string PublicId { get; set; }
        public string Folder { get; set; }
        public string FilePath { get; set; }
        public string PlatformsFilePath { get; set; }

        public DataSentinel(string connectionId)
        {
            PublicId = connectionId;
            Folder = @"publicdata\";
            FilePath = Folder + PublicId + ".json";
            PlatformsFilePath = Folder + PublicId + "-platform.json";

            CreateDataFolderIfNotExists();
            CleanUpDataFiles();
        }

        public Games GetAllGames()
        {
            Games games = this.ConvertJSONToGames();
            Platforms platforms = this.ConvertJSONToPlatforms();
            return this.StitchPlatformsOntoGames(games, platforms);
        }

        private Games StitchPlatformsOntoGames(Games games, Platforms platforms)
        {
            games.GamesList.ForEach(g => {
                g.platformIds.ToList().ForEach(gpi =>
                {
                    Platform selectedPlatform = platforms.PlatformsList.Find(p => p.PlatformId == gpi);
                    g.Platforms.Add(selectedPlatform);
                });
            });

            return games;
        }

        private Game StitchPlatformsOntoGame(Game game, Platforms platforms)
        {
            game.platformIds.ToList().ForEach(gpi =>
            {
                Platform selectedPlatform = platforms.PlatformsList.Find(p => p.PlatformId == gpi);
                game.Platforms.Add(selectedPlatform);
            });

            return game;
        }

        public Game GetGameById(int id)
        {
            Games games = this.GetAllGames();
            Game game = games.GamesList.FirstOrDefault(g => g.GameId == id);

            return game;
        }

        private Games ConvertJSONToGames()
        {
            CreateDataFilesIfNotExists();
            string jsonContent = File.ReadAllText(this.FilePath);
            Games games = JsonConvert.DeserializeObject<Games>(jsonContent);

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

        public List<Platform> GetPlatformsByGameId(int id)
        {
            Games games = this.ConvertJSONToGames();
            Game game = games.GamesList.FirstOrDefault(g => g.GameId == id);
            Platforms platforms = this.ConvertJSONToPlatforms();
            Game stitchedGame = StitchPlatformsOntoGame(game, platforms);

            return stitchedGame.Platforms;
        }

        public void CreateDataFilesIfNotExists()
        {
            string expectedDataFilePath = this.FilePath;
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

        public Games AddGame(Game newGame)
        {
            newGame.GameId = this.GetNextID().Value;
            newGame.platformIds = new int[0];
            newGame.Platforms = new List<Platform>();

            Games games = this.ConvertJSONToGames();
            games.GamesList.Add(newGame);
            this.WriteNewGameFile(games);

            Platforms platforms = this.ConvertJSONToPlatforms();
            return this.StitchPlatformsOntoGames(games, platforms);
        }

        public Games UpdateGame(Game updatedGame)
        {
            if (updatedGame.GameId == 0)
                return null;

            Games games = this.ConvertJSONToGames();
            Game game = games.GamesList.Find(x => x.GameId == updatedGame.GameId);

            if (game != null)
            {
                if (updatedGame.Title != null)
                    game.Title = updatedGame.Title;
                if (updatedGame.Year > 0)
                    game.Year = updatedGame.Year;

                this.WriteNewGameFile(games);
            }
            Platforms platforms = this.ConvertJSONToPlatforms();
            return this.StitchPlatformsOntoGames(games, platforms);
        }

        public Games DeleteGame(int gameIdToDelete)
        {
            Games games = this.ConvertJSONToGames();
            Game game = games.GamesList.Find(x => x.GameId == gameIdToDelete);

            if (game != null)
            {
                games.GamesList.Remove(game);
                this.WriteNewGameFile(games);

                Platforms platforms = this.ConvertJSONToPlatforms();
                return this.StitchPlatformsOntoGames(games, platforms);
            }
            return null;
        }

        internal void WriteNewGameFile(Games games)
        {
            string json = JsonConvert.SerializeObject(games, Formatting.Indented);
            File.WriteAllText(this.FilePath, json);
        }

        private int? GetNextID()
        {
            int? id = null;
            Games games = this.ConvertJSONToGames();
            games.GamesList.ForEach(game => 
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
