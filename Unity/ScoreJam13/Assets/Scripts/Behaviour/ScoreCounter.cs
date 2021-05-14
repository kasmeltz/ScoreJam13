namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class ScoreCounter : MonoBehaviour
    {
        public float score;
        public float scoretoadd;
        public float BlinkCost;
        
        [SerializeField] Text scoretxt;

        private IEnumerator coroutine;
        protected Playermovement Player { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Player = FindObjectOfType<Playermovement>();

            Player.Blinked += Player_Blinked;
        }

        private void Player_Blinked(object sender, System.EventArgs e)
        {
            score -= BlinkCost;
            if (score < 0)
            {
                Player
                    .Die();
            }
            StartCoroutine(Shake(.1f,.2f));
        }

        // Update is called once per frame
        void Update()
        {
            if (!Playermovement.IsPlaying)
            {
                return;
            }

            score += Time.deltaTime * scoretoadd;
            int roundedscore = Mathf.RoundToInt(score);
            scoretxt.text = roundedscore.ToString();
        }
        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalpos = transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {

                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, y, originalpos.z);


                elapsed += Time.deltaTime;

                yield return null;

            }

            transform.localPosition = originalpos;

        }
    }
}