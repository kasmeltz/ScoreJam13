namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    public class Playermovement : MonoBehaviour
    {
        public float BlinkDistance;
        public float Strafespeed;

        public bool invinceble;
        public float invincebletime;
        public float currenttime;
        

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

            Vector2 topRightCorner = new Vector2(1, 1);

            CameraEdge = Camera
                .main
                .ViewportToWorldPoint(topRightCorner);

            Rigidbody = GetComponent<Rigidbody2D>();
            currenttime = invincebletime;
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

        protected void Update()
        {
            SetVariables();

            if (Input
                .GetKeyDown(KeyCode.Space))
            {
                Blink();
            }

            var pos = transform.position + (Movement * Strafespeed * Time.deltaTime);

            if (!CanMoveHere(pos))
            {
                return;
            }

            transform.position = pos;

            if (invinceble)
            {
                
                if (currenttime > 0)
                {
                    currenttime -= Time.deltaTime;
                }
                else
                {
                    invinceble = false;
                }
            }
        }

        void Blink()
        {
            var pos = transform.position + blink;
            if (!CanMoveHere(pos))
            {
                transform.position = new Vector3(CameraEdge.x,CameraEdge.y, 0);
            }

            transform.position += blink;
            invinceble = true;
            currenttime = invincebletime;
            OnBlinked();
        }

        protected void OnCollisionEnter2D(Collision2D collision)
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