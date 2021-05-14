namespace ScoreJam13Server.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    [ApiController]
    [Route("[controller]")]
    public class HighScoreController : ControllerBase
    {
        private static List<HighScore> HighScores = new List<HighScore>();

        [HttpGet]
        public IEnumerable<HighScore> Get()
        {
            return HighScores;
        }

        [HttpPut]
        public HighScore Put([FromBody]HighScore highScore)
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

            bool addHighScore = false;

            var applicable = HighScores
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
                HighScores
                    .Add(highScore);

                HighScores = HighScores
                    .Take(100)
                    .ToList();
            }

            return highScore;
        }
    }
}
