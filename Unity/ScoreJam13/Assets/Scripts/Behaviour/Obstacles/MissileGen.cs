namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/MissileGen")]
    public class MissileGen : MonoBehaviour
    {
        public GameObject missilesprite;

        Vector3 startpoint;

        public GameObject holder;

        public float spawnTime;

        public bool spawning;

        public int max;

        public int spawned;


        // Start is called before the first frame update
        void Start()
        {
            startpoint = transform.position;
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