using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePlatforms : MonoBehaviour
{
    public GameObject[] platforms;

    public float spawntime;

    public GameObject endpoint;

    Vector3 startpoint;

    public GameObject holder;

    public bool spawning = false;

    // Start is called before the first frame update
    void Start()
    {
        startpoint = transform.position;

        Invoke(nameof(AttemptSpawn), spawntime);
    }

    void Spawn()
    {
        if (spawning)
        {
            int randomplatform = UnityEngine.Random.Range(0, platforms.Length);
            GameObject platform = Instantiate(platforms[randomplatform]);

            float starty = UnityEngine.Random.Range(startpoint.y - 4f, startpoint.y + 4f);
            platform.transform.position = new Vector3(startpoint.x, starty, startpoint.z);

            //float scale = UnityEngine.Random.Range(0.5f, 1f);
            //platform.transform.localScale = new Vector2(scale, scale);

            float alpha = UnityEngine.Random.Range(0.5f, 1f);
            platform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);

            float speed = UnityEngine.Random.Range(0.7f, 3f);

            platform.transform.SetParent(holder.transform);

            platform.GetComponent<Platform>().startFloating(speed, endpoint.transform.position.x);
        }
    }

    void AttemptSpawn()
    {
        Spawn();

        Invoke(nameof(AttemptSpawn), spawntime);
    }
}
