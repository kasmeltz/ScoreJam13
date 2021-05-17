namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/Missile")]
    public class Missile : BehaviourBase
    {
        public float speed = 5;

        Rigidbody2D rb;

        public float rotateSpeed = 200;

        public float lifetime;

        protected Playermovement player;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Playermovement>();
            rb = GetComponent<Rigidbody2D>();
            Invoke(nameof(Destroy), lifetime);
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 direction = (Vector2)player.transform.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }

        private void Destroy()
        {
            DestroyComponent(this);
        }

    }
}
