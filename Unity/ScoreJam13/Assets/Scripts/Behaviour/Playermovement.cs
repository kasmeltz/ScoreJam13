namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    public class Playermovement : MonoBehaviour
    {
        public float BlinkDistance;

        public float Strafespeed;

        protected bool IsBlinking { get; set; }

        protected Vector3 BlinkVector { get; set; }

        public static bool IsPlaying { get; set; }

        protected Vector3 Movement { get; set; }

        protected Vector2 CameraEdge { get; set; }

        protected SpriteRenderer SpriteRenderer { get; set; }

        protected Rigidbody2D Rigidbody { get; set; }

        protected ScoreCounter ScoreCounter { get; set; }

        protected Animator Animator { get; set; }

        protected bool IsDead { get; set; }

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

        #region Animation Callbacks

        public void BlinkStarted()
        {
        }

        public void BlinkEnded()
        {
            IsBlinking = false;
        }

        #endregion

        public void Reset()
        {
            IsDead = false;
            transform.position = Vector3.zero;
        }

        void Start()
        {
            ScoreCounter = FindObjectOfType<ScoreCounter>();

            SpriteRenderer = GetComponent<SpriteRenderer>();

            Rigidbody = GetComponent<Rigidbody2D>();

            Animator = GetComponent<Animator>();

            Reset();
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
            if (IsDead)
            {
                return;
            }
            
            IsDead = true;

            transform.position = Vector2.zero;

            Rigidbody
                .MovePosition(Vector2.zero);

            OnDied();
            FindObjectOfType<AudioManager>().Playoneshot("Death");
        }

        protected void Update()
        {
            if (!IsPlaying)
            {
                return;
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
            if (ScoreCounter.score < ScoreCounter.BlinkCost)
            {
                FindObjectOfType<AudioManager>().Playoneshot("FailedBlink");
                return;
            }            

            var pos = transform.position + BlinkVector;
            transform.position = GetMoveHere(pos);

            FindObjectOfType<AudioManager>().Playoneshot("BlinkS");

            Animator
                .SetTrigger("Blink");

            
            IsBlinking = true;
            
            
            OnBlinked();
        }
        
        protected void OnCollisionStay2D(Collision2D collision)
        {
            if(!IsBlinking)
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

            var dir = movement.normalized;

            Animator
                .SetFloat("X", dir.x);

            Animator
                .SetFloat("Y", dir.y);

            if (Movement.x > 0)
            {
                BlinkVector = new Vector3(BlinkDistance, 0, 0);
            }
            if (Movement.x < 0)
            {
                BlinkVector = new Vector3(-BlinkDistance, 0, 0);
            }
            if (Movement.y > 0)
            {
                BlinkVector = new Vector3(0, BlinkDistance, 0);
            }
            if (Movement.y < 0)
            {
                BlinkVector = new Vector3(0, -BlinkDistance, 0);
            }
        }

        
    }
}