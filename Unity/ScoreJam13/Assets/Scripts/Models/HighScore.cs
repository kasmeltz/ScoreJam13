namespace KasJam.ScoreJam13.Unity.Models
{
    using System;

    [Serializable]
    public class HighScore
    {
        public long fileTime;

        public string playerName;

        public int score;
        
        public string checkSum;

        public bool cheating;
    }
}