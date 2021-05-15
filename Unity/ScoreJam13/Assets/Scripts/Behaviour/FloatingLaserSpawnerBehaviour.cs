namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/FloatingLaserSpawner")]
    public class FloatingLaserSpawnerBehaviour : BehaviourBase
    {
        

        #region Members

        public GameObject Holder;

        public GameObject FloatingLaserPrefab;

        Playermovement Player;

        public float SpawnFrequency;

        protected float SpawnCounter { get; set; }

        protected ScoreCounter ScoreCounter { get; set; }

        public bool spawning = false;

        public bool Rotating = false;

        #endregion

        #region Protected Methods
        protected void Reset()
        {
            SpawnCounter = SpawnFrequency;
            Player = FindObjectOfType<Playermovement>();
        }

        protected void SpawnFloatingLaser()
        {
            if (spawning)
            {
                var laser = Instantiate(FloatingLaserPrefab);
                laser
                    .transform
                    .SetParent(Holder.transform);

                var fl = laser.GetComponent<FloatingLaserBehaviour>();
                fl.Rotating = Rotating;

                float xPos = Player.transform.position.x;

                laser.transform.position = new Vector3(xPos, 6f, 0);
            }
            
        }

        #endregion

        #region Unity


        protected void Update()
        {
            if (!Playermovement.IsPlaying)
            {
                return;
            }

            SpawnCounter -= Time.deltaTime;
            if (SpawnCounter <= 0)
            {
                SpawnCounter = SpawnFrequency;
                SpawnFloatingLaser();
            }
        }

        protected override  void Awake()
        {
            base
                .Awake();

            ScoreCounter = FindObjectOfType<ScoreCounter>();

            Reset();            
        }

        #endregion
    }
}
