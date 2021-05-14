namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using KasJam.ScoreJam13.Unity.Models;
    using System;
    using System.Collections;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;

    [AddComponentMenu("AScoreJam13/HighScoreSender")]
    public class HighScoreSenderBehaviour : BehaviourBase
    {
        protected static string WebApiURL = "http://localhost:63193/highscore";

        public IEnumerator SendHighScore(HighScore highScore)
        {
            using (var md5 = MD5
                .Create())
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
                
                highScore.CheckSum = sb.ToString();
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

            StartCoroutine(SendHighScore(new HighScore
            {
                PlayerName = "kasmeltz",
                FileTime = DateTime.UtcNow.ToFileTimeUtc(),
                Score = 1900
            }));
        }
    }
}