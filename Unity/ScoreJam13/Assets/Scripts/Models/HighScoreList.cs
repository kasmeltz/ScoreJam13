namespace KasJam.ScoreJam13.Unity.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class HighScoreList
    {
        public HighScoreList()
        {
            scores = new List<HighScore>();
        }

        public List<HighScore> scores;
    }
}