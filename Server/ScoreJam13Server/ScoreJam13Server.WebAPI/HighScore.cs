namespace ScoreJam13Server.WebAPI
{
    public class HighScore
    {
        public long FileTime { get; set; }

        public string PlayerName { get; set; }

        public int Score { get; set; }

        public string CheckSum { get; set; }

        public bool Cheating { get; set; }
    }
}
