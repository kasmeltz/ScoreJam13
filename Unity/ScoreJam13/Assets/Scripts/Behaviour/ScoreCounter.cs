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

        public enum levels{lvl1, lvl2, lvl3, lvl4}
        levels levelstate;

        public int lvlpoint1 = 1000; 
        public int lvlpoint2 = 2000; 
        public int lvlpoint3 = 3000; 
        public int lvlpoint4 = 4000;

        private SidePlatforms sidePlatforms;
        private FloatingLaserSpawnerBehaviour laser;
        
        [SerializeField] Text scoretxt;

        
        protected Playermovement Player { get; set; }

        protected ScrollingMapGeneratorBehvaiour MapGenerator { get; set; }


        // Start is called before the first frame update
        void Start()
        {
            levelstate = levels.lvl1;

            Player = FindObjectOfType<Playermovement>();

            if (Player != null)
            {
                Player.Blinked += Player_Blinked;
            }
            MapGenerator = FindObjectOfType<ScrollingMapGeneratorBehvaiour>();

            sidePlatforms = FindObjectOfType<SidePlatforms>();
            laser = FindObjectOfType<FloatingLaserSpawnerBehaviour>();
        }

        private void Player_Blinked(object sender, System.EventArgs e)
        {
            score -= BlinkCost;
            if (score < 0)
            {
                score = 0;
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

            score += Time.deltaTime * scoretoadd * MapGenerator.ActualScrollSpeed;
            int roundedscore = Mathf.RoundToInt(score);
            scoretxt.text = roundedscore.ToString();

            if (score >= lvlpoint1)
            {
                levelstate = levels.lvl2;
            }

            if (levelstate == levels.lvl2)
            {
                sidePlatforms.spawning = true;
            }
            else
            {
                sidePlatforms.spawning = false;
            }

            //

            if (score >= lvlpoint2)
            {
                levelstate = levels.lvl3;
            }

            if (levelstate == levels.lvl3)
            {
                laser.spawning = true;
            }
            else
            {
                laser.spawning = false;
            }

            //

            //if (score >= lvlpoint3)
            //{
            //    levelstate = levels.lvl4;
            //}

            //if (levelstate == levels.lvl4)
            //{
            //    sidePlatforms.spawning = true;
            //}


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