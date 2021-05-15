namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    public class Playermovement : BehaviourBase
    {
        public float BlinkDistance;

        public float Strafespeed;

        public float CoinScore;

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
            BlinkVector = new Vector3(0, 1, 0);
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
            
            AudioManager.Playoneshot("Death");
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
                AudioManager.Playoneshot("FailedBlink");
                return;
            }            

            var pos = transform.position + BlinkVector;
            transform.position = GetMoveHere(pos);

            Animator
                .SetTrigger("Blink");

            AudioManager.Playoneshot("BlinkS");
            
            IsBlinking = true;            
            
            OnBlinked();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            var coin = collision
                .GetComponent<CoinBehaviour>();    

            if (coin != null)
            {
                ScoreCounter.score += CoinScore;

                DestroyComponent(coin);
            }

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

            if (Mathf.Abs(dir.x) > 0.3f || Mathf.Abs(dir.y) > 0.3f)
            { 
                BlinkVector = dir * BlinkDistance;

                Animator
                    .SetFloat("X", dir.x);

                Animator
                    .SetFloat("Y", dir.y);
            }
        }        
    }
}