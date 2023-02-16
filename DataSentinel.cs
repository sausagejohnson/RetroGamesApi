using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System;

namespace RetroGamesApi
{
    public class DataSentinel
    {
        public string PublicId { get; set; }
        public string Folder { get; set; }
        public string FilePath { get; set; }

        public DataSentinel(string connectionId)
        {
            PublicId = connectionId;
            Folder = @"publicdata\";
            FilePath = Folder + PublicId + ".json";

            CreateDataFolderIfNotExists();
            CleanUpDataFiles();
        }

        public Games ConvertJSONToGames()
        {
            CreateDataFileIfNotExists();
            string jsonContent = File.ReadAllText(this.FilePath);
            Games games = JsonConvert.DeserializeObject<Games>(jsonContent);

            return games;
        }

        private string GetRawDataFile()
        {
            CreateDataFileIfNotExists();
            string jsonContent = File.ReadAllText(this.FilePath);
            return jsonContent;
        }

        public void CreateDataFileIfNotExists()
        {
            string expectedDataFilePath = this.FilePath;
            if (!File.Exists(expectedDataFilePath))
            {
                File.Copy("games.json", expectedDataFilePath);
            }
        }

        public Games AddGame(Game newGame)
        {
            newGame.GameId = this.GetNextID().Value;

            Games games = this.ConvertJSONToGames();
            games.GamesList.Add(newGame);
            this.WriteNewGameFile(games);

            return games;
        }

        public Games UpdateGame(Game updatedGame)
        {
            Games games = this.ConvertJSONToGames();
            Game game = games.GamesList.Find(x => x.GameId == updatedGame.GameId);

            if (game != null)
            {
                game.Title = updatedGame.Title;
                game.Year = updatedGame.Year;

                this.WriteNewGameFile(games);
            }
            return games;
        }

        public Games DeleteGame(int gameIdToDelete)
        {
            Games games = this.ConvertJSONToGames();
            Game game = games.GamesList.Find(x => x.GameId == gameIdToDelete);

            if (game != null)
            {
                games.GamesList.Remove(game);
                this.WriteNewGameFile(games);
                //return true;
                return games;
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
