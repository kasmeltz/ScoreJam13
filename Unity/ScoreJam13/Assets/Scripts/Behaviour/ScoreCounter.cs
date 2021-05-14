namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreCounter : MonoBehaviour
    {
        public float score;
        public float scoretoadd;
        public float BlinkCost;

        [SerializeField] Text scoretxt;

        protected Playermovement Player { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Player = FindObjectOfType<Playermovement>();

            Player.Blinked += Player_Blinked;
            Player.Died += Player_Died;
        }

        private void Player_Died(object sender, System.EventArgs e)
        {
            score = 0;
        }

        private void Player_Blinked(object sender, System.EventArgs e)
        {
            score -= BlinkCost;
            if (score < 0)
            {
                Player
                    .Die();
            }
        }

        // Update is called once per frame
        void Update()
        {
            score += Time.deltaTime * scoretoadd;
            int roundedscore = Mathf.RoundToInt(score);
            scoretxt.text = roundedscore.ToString();
        }
    }
}