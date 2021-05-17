namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    public class MissileGen : MonoBehaviour
    {
        public GameObject missilesprite;

        Vector3 startpoint;

        public GameObject holder;

        public float spawnTime;

        public bool spawning;


        // Start is called before the first frame update
        void Start()
        {
            startpoint = transform.position;
            Invoke(nameof(AttemptSpawn), spawnTime);
        }

        void Spawn()
        {
            if (spawning)
            {
                GameObject missile = Instantiate(missilesprite);
                missile.transform.position = new Vector3(Random.Range(startpoint.x - 10, startpoint.x + 10), startpoint.y, startpoint.z);
                missile.transform.SetParent(holder.transform);
            }
        }

        void AttemptSpawn()
        {
            Spawn();
            Invoke(nameof(AttemptSpawn), spawnTime);
        }
    }
}