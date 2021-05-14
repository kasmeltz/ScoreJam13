namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    public class Playermovement : MonoBehaviour
    {
        public float BlinkDistance;
        public float Strafespeed;

        public Vector3 blink;

        protected Vector3 Movement { get; set; }

        protected Vector2 CameraEdge { get; set; }

        protected SpriteRenderer SpriteRenderer { get; set; }

        protected Rigidbody2D Rigidbody { get; set; }

        #region Events

        public event EventHandler Blinked;

        protected void OnBlinked()
        {
            Blinked?
                .Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Died;

        protected void OnDied()
        {
            Died?
                .Invoke(this, EventArgs.Empty);
        }

        #endregion

        void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected Vector3 GetMoveHere(Vector3 pos)
        {
            var w = (SpriteRenderer.sprite.rect.width * transform.localScale.x) / 100;
            var h = (SpriteRenderer.sprite.rect.height * transform.localScale.y) / 100;

            if (pos.x > CameraEdge.x - w)
            {
                pos.x = CameraEdge.x - w;
            }

            if (pos.x < -CameraEdge.x + w)
            {
                pos.x = -CameraEdge.x + w;
            }

            if (pos.y > CameraEdge.y - h)
            {
                pos.y = CameraEdge.y - h;
            }

            if (pos.y < -CameraEdge.y + h)
            {
                pos.y = -CameraEdge.y + h;
            }

            return pos;
        }

        public void Die()
        {
            transform.position = Vector2.zero;

            Rigidbody
                .MovePosition(Vector2.zero);

            OnDied();
        }

        protected void Update()
        {
            Vector2 topRightCorner = new Vector2(1, 1);

            CameraEdge = Camera
                .main
                .ViewportToWorldPoint(topRightCorner);

            SetVariables();

            if (Input
                .GetKeyDown(KeyCode.Space))
            {
                Blink();
            }

            var pos = transform.position + (Movement * Strafespeed * Time.deltaTime);
            transform.position = GetMoveHere(pos);
        }

        void Blink()
        {
            var pos = transform.position + blink;
            transform.position = GetMoveHere(pos);

            if (transform.position.x != pos.x || transform.position.y != pos.y)
            {
                OnBlinked();
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            Die();
        }

        private void SetVariables()
        {
            var movement = Movement;
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            Movement = movement;

            if (Movement.x > 0)
            {
                blink = new Vector3(BlinkDistance, 0, 0);
            }
            if (Movement.x < 0)
            {
                blink = new Vector3(-BlinkDistance, 0, 0);
            }
            if (Movement.y > 0)
            {
                blink = new Vector3(0, BlinkDistance, 0);
            }
            if (Movement.y < 0)
            {
                blink = new Vector3(0, -BlinkDistance, 0);
            }
        }
    }
}