namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    public class Playermovement : MonoBehaviour
    {
        public static bool IsPlaying { get; set; }

        public float BlinkDistance;
        public float Strafespeed;

        public float invincebletime;
        
        protected float invincebleCountdown { get; set; }

        protected bool invinceble { get; set; }

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
            if (!IsPlaying)
            {
                return;
            }

            if (invinceble)
            {
                if (invincebleCountdown > 0)
                {
                    invincebleCountdown -= Time.deltaTime;
                    if (invincebleCountdown <= 0)
                    {
                        MakeInvincible(false);
                    }
                }
            }

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

            
            OnBlinked();
            transform.position += blink;

            MakeInvincible(true);
        }

        protected void MakeInvincible(bool isInvincible)
        {
            if (isInvincible)
            {
                invinceble = true;
                invincebleCountdown = invincebletime;
            }
            else
            {
                invinceble = false;
                invincebleCountdown = 0;
            }
        }

        protected void OnCollisionStay2D(Collision2D collision)
        {
            if(!invinceble)
            {
                Die();
            }            
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