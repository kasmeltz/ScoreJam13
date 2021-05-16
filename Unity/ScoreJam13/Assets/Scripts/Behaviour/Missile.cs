namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Missile : MonoBehaviour
    {

        public float speed = 5;

        Rigidbody2D rb;

        public float rotateSpeed = 200;

        protected Playermovement player;


    // Start is called before the first frame update
    void Start()
        {
            player = FindObjectOfType<Playermovement>();
            rb = GetComponent<Rigidbody2D>();
            //Physics2D.IgnoreLayerCollision(6, 7);
            Invoke(nameof(Destroy), 10);

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
            Destroy(this.gameObject);
        }

    }
}