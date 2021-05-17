namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreCounter : BehaviourBase
    {
        public float score;
        public float scoretoadd;
        public float BlinkCost;

        public float[] LevelScoresRequired;

        public int Level { get; protected set; }

        private SidePlatforms sidePlatforms;
        private FloatingLaserSpawnerBehaviour laser;
        
        [SerializeField] Text scoretxt;
        
        protected Playermovement Player { get; set; }

        protected ScrollingMapGeneratorBehvaiour MapGenerator { get; set; }

        protected MissileGen missile { get; set; }

        public void Reset()
        {
            Level = 0;
            if (sidePlatforms != null)
            {
                sidePlatforms.spawning = false;
            }
            if (laser != null)
            {
                laser.spawning = false;
                laser.Rotating = false;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Player = FindObjectOfType<Playermovement>();
            if (Player != null)
            {
                Player.Blinked += Player_Blinked;
            }

            MapGenerator = FindObjectOfType<ScrollingMapGeneratorBehvaiour>();
            sidePlatforms = FindObjectOfType<SidePlatforms>();
            laser = FindObjectOfType<FloatingLaserSpawnerBehaviour>();
            missile = FindObjectOfType<MissileGen>();
            if (FindObjectOfType<Mainmenu>() == null)
            {
                Reset();
            }
        }

        private void Player_Blinked(object sender, System.EventArgs e)
        {
            StartCoroutine(Shake(.1f,.2f));
        }

        // Update is called once per frame
        void Update()
        {
            if (!Player.IsPlaying)
            {
                return;
            }

            score += Time.unscaledDeltaTime * scoretoadd * MapGenerator.ActualScrollSpeed;
            int roundedscore = Mathf.RoundToInt(score);
            scoretxt.text = roundedscore.ToString();

            Level = 0;
            for (int i = 0; i < LevelScoresRequired.Length; i++)
            {
                if (score >= LevelScoresRequired[i])
                {
                    Level = i;
                }
                else
                {
                    break;
                }
            }

            sidePlatforms.spawning = Level >= 1;
            laser.spawning = Level >= 2;
            MapGenerator.SpawnBlinkTiles = Level >= 3;
            missile.spawning = Level >= 4;
            MapGenerator.SpawnBombs = Level >= 5;
            MapGenerator.SpawnFakeFloors = Level >= 6;
            laser.Rotating = Level >= 7;
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