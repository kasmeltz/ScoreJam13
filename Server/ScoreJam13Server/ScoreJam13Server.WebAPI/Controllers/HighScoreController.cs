namespace ScoreJam13Server.WebAPI.Controllers
{
    using Azure.Storage.Blobs;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class HighScoreController : ControllerBase
    {
        public HighScoreController()
        {
            if (BlobServiceClient == null)
            {
                BlobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=scorejam13storage;AccountKey=37CB7czNGdfEPOUsaqydkhcLgsVwusWaj5asgXEqDzU5Uu5g1tTy0J0qo5veIcNXrjjA2C3bK7cpsRf2nqTAWg==;EndpointSuffix=core.windows.net");
            }

            if (JsonSettings == null)
            {
                JsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
            }
        }

        protected static BlobServiceClient BlobServiceClient { get; set; }

        protected static JsonSerializerSettings JsonSettings { get; set; }

        [HttpGet]
        public async Task<IEnumerable<HighScore>> Get()
        {
            return await GetHighScoresFromBlob();
        }

        [HttpPut]
        public async Task<HighScore> Put([FromBody] HighScore highScore)
        {
            using (var md5 = MD5.Create())
            {
                string toHash = $"{highScore.PlayerName}{highScore.Score}{highScore.FileTime}";

                byte[] toHashBytes = Encoding
                    .ASCII
                    .GetBytes(toHash);

                var hashBytes = md5
                    .ComputeHash(toHashBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                var hashString = sb.ToString();

                if (hashString != highScore.CheckSum)
                {
                    highScore.Cheating = true;
                }
            }

            var highScores = await GetHighScoresFromBlob();

            bool addHighScore = false;

            var applicable = highScores
                .Where(o => o.Cheating == highScore.Cheating)
                .OrderBy(o => o.Score)
                .ToList();

            if (applicable.Count < 50)
            {
                addHighScore = true;
            }
            else if (highScore.Score > applicable[0].Score)
            {
                addHighScore = true;
            }

            if (addHighScore)
            {
                highScores
                    .Add(highScore);

                highScores = highScores
                    .OrderByDescending(o => o.Score)
                    .Take(100)
                    .ToList();
            }

            await StoreHighScoreInBlob(highScores);

            return highScore;
        }

        protected async Task StoreHighScoreInBlob(List<HighScore> highScores)
        {
            string json = JsonConvert
                .SerializeObject(highScores, Formatting.None, JsonSettings);

            var containerClient = BlobServiceClient
                .GetBlobContainerClient("highscores");

            var blobClient = containerClient
                .GetBlobClient("data.txt");

            using (MemoryStream ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms, Encoding.UTF8);
                try
                {
                    sw
                        .Write(json);

                    sw
                        .Flush();

                    ms
                        .Seek(0, SeekOrigin.Begin);

                    await blobClient
                        .UploadAsync(sw.BaseStream, true);
                }
                finally
                {
                    sw.Dispose();
                }
            }
        }

        protected async Task<List<HighScore>> GetHighScoresFromBlob()
        {
            try
            {
                var containerClient = BlobServiceClient
                    .GetBlobContainerClient("highscores");

                var blobClient = containerClient
                    .GetBlobClient("data.txt");

                var download = await blobClient
                    .DownloadAsync();

                using (var sr = new StreamReader(download.Value.Content))
                {
                    string json = sr
                        .ReadToEnd();

                    return JsonConvert
                        .DeserializeObject<List<HighScore>>(json, JsonSettings);
                }
            }
            catch
            {
                return new List<HighScore>();
            }
        }
    }
}
