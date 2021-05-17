namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/MissileGen")]
    public class MissileGen : BehaviourBase
    {
        public GameObject missilesprite;

        Vector3 startpoint;

        public GameObject holder;

        public float spawnTime;

        public bool spawning;

        public int max;

        public int spawned;

        protected Playermovement Player { get; set;  }

        public void Reset()
        {
            spawning = false;
            startpoint = transform.position;
            spawned = 0;
        }

        protected override void Awake()
        {
            base
                .Awake();

            Player = FindObjectOfType<Playermovement>();
            Player.Died += Player_Died;
        }

        private void Player_Died(object sender, System.EventArgs e)
        {
            Reset();
        }

        // Start is called before the first frame update
        void Start()
        {
            Reset();
            Invoke(nameof(AttemptSpawn), spawnTime);
        }

        void Spawn()
        {
            if (spawning & spawned < max)
            {
                GameObject missile = Instantiate(missilesprite);
                missile.transform.position = new Vector3(Random.Range(startpoint.x - 10, startpoint.x + 10), startpoint.y, startpoint.z);
                missile.transform.SetParent(holder.transform);
                spawned += 1;
            }
        }

        void AttemptSpawn()
        {
            Spawn();
            Invoke(nameof(AttemptSpawn), spawnTime);
        }
    }
}