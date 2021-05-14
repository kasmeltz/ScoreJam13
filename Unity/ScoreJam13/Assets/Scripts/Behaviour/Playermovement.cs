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

        // Start is called before the first frame update
        void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            Vector2 topRightCorner = new Vector2(1, 1);

            CameraEdge = Camera
                .main
                .ViewportToWorldPoint(topRightCorner);

            Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected bool CanMoveHere(Vector3 pos)
        {
            var w = 4 * SpriteRenderer.sprite.rect.width / 100;
            var h = 4 * SpriteRenderer.sprite.rect.height / 100;

            if (pos.x > CameraEdge.x - w)
            {
                return false;
            }

            if (pos.x < -CameraEdge.x + w)
            {
                return false;
            }

            if (pos.y > CameraEdge.y - h)
            {
                return false;
            }

            if (pos.y < -CameraEdge.y + h)
            {
                return false;
            }

            return true;                
        }

        public void Die()
        {
            transform.position = Vector2.zero;

            Rigidbody
                .MovePosition(Vector2.zero);

            OnDied();
        }

        protected void FixedUpdate()
        {
            SetVariables();

            if (Input
                .GetKeyDown(KeyCode.Space))
            {
                Blink();
            }

            //var pos = transform.position + (Movement * Strafespeed * Time.deltaTime);
            var pos = transform.position + (Movement * Strafespeed * Time.fixedDeltaTime);

            if (!CanMoveHere(pos))
            {
                return;
            }

            Rigidbody
                .MovePosition(pos);
        }

        void Blink()
        {
            var pos = transform.position + blink;
            if (!CanMoveHere(pos))
            {
                return;
            }

            /*
            if (Physics
                .Raycast(transform.position, blink.normalized, out RaycastHit hitInfo, BlinkDistance))
            {
                if (hitInfo.collider != null)
                {
                    return;
                }
            }
            */

            transform.position += blink;

            OnBlinked();
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