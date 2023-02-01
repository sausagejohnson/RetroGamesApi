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

        public DataSentinel(HttpContext context)
        {
            PublicId = context.Connection.Id;
            Folder = @"publicdata\";
            FilePath = Folder + PublicId + ".json";

            CleanUpDataFiles();
        }

        public Games ConvertRawToGames()
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

        internal void WriteNewGameFile(Games games)
        {
            string json = JsonConvert.SerializeObject(games, Formatting.Indented);
            File.WriteAllText(this.FilePath, json);
        }

        private void CleanUpDataFiles()
        {
            string[] userDataFiles = Directory.GetFiles(this.Folder);
            foreach (string file in userDataFiles)
            {
                DateTime createdTime = File.GetCreationTimeUtc(file);
                DateTime nowUTC = DateTime.UtcNow;

                TimeSpan difference = nowUTC.Subtract(createdTime);
                if (difference.Seconds > 300)
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
