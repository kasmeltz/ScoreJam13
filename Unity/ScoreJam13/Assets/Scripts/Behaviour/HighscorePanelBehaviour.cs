namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using KasJam.ScoreJam13.Unity.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("AScoreJam13/HighscorePanel")]
    public class HighscorePanelBehaviour : BehaviourBase
    {


        #region Members

        public Text NonCheatersText;

        public Text CheatersText;

        public Text finalscore;

        protected float PersonalBest { get; set; }

        protected HighScoreHandlerBehaviour HighScoreHandler { get; set; }

        protected ScoreCounter ScoreCounter { get; set; }

        protected ScrollingMapGeneratorBehvaiour ScrollingMapGenerator { get; set; }


        #endregion

        #region Event Handlers

        public void SetPlayerName(string name)
        {
            Playermovement.PlayerName = name;
        }

        private void Player_Died(object sender, System.EventArgs e)
        {
            gameObject
                .SetActive(true);

            Playermovement.IsPlaying = false;

            finalscore.text = (Mathf.RoundToInt(ScoreCounter.score)).ToString();

            if (ScoreCounter.score > PersonalBest)
            {
                SaveScore(ScoreCounter.score);
                PersonalBest = ScoreCounter.score;
            }
            
            StartCoroutine(SendHighScore());
        }

        #endregion

        #region Public Methods

        public void StartDaGame()
        {
            ScoreCounter.score = 0;
            ScrollingMapGenerator
                .Reset();
            
            var player = FindObjectOfType<Playermovement>();
            player
                .Reset();

            gameObject
                .SetActive(false);

            Playermovement.IsPlaying = true;            
        }

        #endregion

        #region Protected Methods

        protected IEnumerator SendHighScore()
        {
            HighScore highScore = new HighScore
            {
                fileTime = DateTime.UtcNow.ToFileTime(),
                playerName = Playermovement.PlayerName,
                score = Mathf.RoundToInt(ScoreCounter.score)
            };

            yield return HighScoreHandler.SendHighScore(highScore);

            yield return UpdateScoreTable();
        }

        protected void SetHighScoreList(List<HighScore> scores, Text text)
        {
            StringBuilder sb = new StringBuilder();

            foreach(var score in scores)
            {
                int length = Math.Min(score.playerName.Length, 10);

                sb
                    .Append($"{score.playerName.Substring(0, length),10}");
                
                sb
                    .Append("\t");

                sb.Append($"{score.score,9}");

                sb
                    .Append("\t");

                var date = DateTime.FromFileTime(score.fileTime);

                sb
                    .Append($"{date.ToLongDateString(),20}");

                sb.AppendLine();
            }

            text.text = sb.ToString();
        }

        protected IEnumerator UpdateScoreTable()
        {
            yield return HighScoreHandler
                .LoadHighScores();

            var nonCheaters = HighScoreHandler
                .HighScores
                .scores
                .Where(o => !o.cheating)
                .OrderByDescending(o => o.score)
                .Take(25)
                .ToList();

            var cheaters = HighScoreHandler
                .HighScores
                .scores
                .Where(o => o.cheating)
                .OrderByDescending(o => o.score)
                .Take(25)
                .ToList();

            SetHighScoreList(nonCheaters, NonCheatersText);
            SetHighScoreList(cheaters, CheatersText);
        }

        protected void SaveScore(float score)
        {
            var path = Path
                .Combine(Application.persistentDataPath, "pb.txt");

            File
                .WriteAllText(path, $"{score}");
        }

        protected float LoadScore()
        {
            var path = Path
                .Combine(Application.persistentDataPath, "pb.txt");

            if (File
                .Exists(path))
            {
                string s = File
                    .ReadAllText(path);

                float
                    .TryParse(s, out float f);

                return f;
            }

            return 0;
        }

        #endregion

        #region Unity

        protected void OnEnable()
        {
            Playermovement.IsPlaying = false;

            HighScoreHandler = FindObjectOfType<HighScoreHandlerBehaviour>();
            ScoreCounter = FindObjectOfType<ScoreCounter>();
            ScrollingMapGenerator = FindObjectOfType<ScrollingMapGeneratorBehvaiour>();

            StartCoroutine(UpdateScoreTable());
        }

        protected override void Awake()
        {
            var player = FindObjectOfType<Playermovement>();
            player.Died += Player_Died;

            PersonalBest = LoadScore();
        }
      
        #endregion
    }
}