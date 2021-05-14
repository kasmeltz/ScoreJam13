namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using KasJam.ScoreJam13.Unity.Models;
    using System.Collections;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;

    [AddComponentMenu("AScoreJam13/HighScoreSender")]
    public class HighScoreHandlerBehaviour : BehaviourBase
    {
        protected static string WebApiURL = "https://scorejam13.azurewebsites.net/highscore";

        protected HighScoreList HighScores { get; set; }

        public IEnumerator GetHighScores()
        {
            using (UnityWebRequest www = UnityWebRequest
                .Get(WebApiURL))
            {
                yield return www
                    .SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug
                        .Log(www.error);
                }
                else
                {

                    Debug
                        .Log("Highscore download complete!");

                    string json = $"{{\"scores\": {www.downloadHandler.text} }}";

                    HighScores = JsonUtility
                        .FromJson<HighScoreList>(json);
                }
            }
        }

        public IEnumerator SendHighScore(HighScore highScore)
        {
            using (var md5 = MD5
                .Create())
            {
                string toHash = $"{highScore.playerName}{highScore.score}{highScore.fileTime}";

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
                
                highScore.checkSum = sb.ToString();
            }

            string json = JsonUtility
                .ToJson(highScore);

            using (UnityWebRequest www = UnityWebRequest
                .Put(WebApiURL, json))
            {
                www
                    .SetRequestHeader("Content-Type", "application/json");

                yield return www
                    .SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug
                        .Log(www.error);
                }
                else
                {
                    Debug
                        .Log("Highscore upload complete!");
                }
            }
        }

        protected override void Awake()
        {
            base
                .Awake();

            HighScores = new HighScoreList();

            StartCoroutine(GetHighScores());
        }
    }
}