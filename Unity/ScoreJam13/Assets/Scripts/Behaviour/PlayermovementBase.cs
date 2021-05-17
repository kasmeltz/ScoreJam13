namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("AScoreJam13/PlayerCharacterBase")]
    public class PlayermovementBase : BehaviourBase
    {
        public float BlinkDistance;

        public float Strafespeed;

        public Image ExtraLifePanel;

        public Image ExtraLifePrefab;

        protected int ExtraLives { get; set; }

        protected bool IsDead { get; set; }

        protected bool IsBlinking { get; set; }

        public Vector3 BlinkVector { get; set; }

        protected Vector3 Movement { get; set; }

        protected Vector2 CameraEdge { get; set; }

        protected SpriteRenderer SpriteRenderer { get; set; }

        protected BoxCollider2D Collider { get; set; }

        protected Rigidbody2D Rigidbody { get; set; }

        protected Animator Animator { get; set; }

        protected ScoreCounter ScoreCounter { get; set; }

        protected Vector2 OldColliderSize { get; set; }

        public bool IsPlaying { get; protected set; }

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
            IsBlinking = true;            
            Collider.size = new Vector2(0, 0);
        }

        public void BlinkEnded()
        {
            IsBlinking = false;
            Collider.size = OldColliderSize;
        }

        #endregion

        #region Public Methods

        public void SetIsPlaying(bool isPlaying)
        {
            IsPlaying = isPlaying;

            ExtraLifePanel
                .gameObject
                .SetActive(isPlaying);
        }

        public void SetImmune()
        {
            IsBlinking = true;

            var pos = transform.position + BlinkVector;
            transform.position = GetMoveHere(pos);

            Animator
                .ResetTrigger("Blink");

            Animator
                .SetTrigger("Immune");

            AudioManager
                .Playoneshot("BlinkS");

            OnBlinked();
        }

        public void Die()
        {
            if (IsDead)
            {
                return;
            }

            if (ExtraLives > 0)
            {
                ExtraLives--;

                UpdateExtraLives();

                SetImmune();

                return;
            }

            IsDead = true;

            transform.position = Vector2.zero;

            Animator
                .ResetTrigger("Blink");

            Animator
                .ResetTrigger("Immune");

            OnDied();

            AudioManager
                .Playoneshot("Death");
        }

        public void GainLife()
        {
            ExtraLives++;

            UpdateExtraLives();
        }

        public void Blink()
        {
            if (IsBlinking)
            {
                return;
            }

            var pos = transform.position + BlinkVector;
            transform.position = GetMoveHere(pos);

            Animator
                .SetTrigger("Blink");

            AudioManager
                .Playoneshot("BlinkS");

            IsBlinking = true;

            OnBlinked();
        }

        #endregion

        #region Protected Methods

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

        protected void UpdateExtraLives()
        {
            foreach (Transform transform in ExtraLifePanel.transform)
            {
                DestroyComponent(transform);
            }

            float sx = 20;
            float sy = 0;
            for (int i = 0; i < ExtraLives; i++)
            {
                var extraLife = Instantiate(ExtraLifePrefab);
                extraLife
                    .transform
                    .SetParent(ExtraLifePanel.rectTransform);

                extraLife.rectTransform.anchoredPosition = new Vector2(sx, sy);

                sx += 62;
            }
        }

        protected void SetVariables()
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
            else
            {
                BlinkVector = Vector2.up * BlinkDistance;

                Animator
                    .SetFloat("X", 0);

                Animator
                    .SetFloat("Y", 1);
            }
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<BoxCollider2D>();
            Animator = GetComponent<Animator>();
            ScoreCounter = FindObjectOfType<ScoreCounter>();

            if (ExtraLifePanel != null)
            {
                ExtraLifePanel
                    .gameObject
                    .SetActive(true);
            }

            OldColliderSize = Collider.size;
        }

        protected virtual void Update()
        {
            Vector2 topRightCorner = new Vector2(1, 1);

            CameraEdge = Camera
                .main
                .ViewportToWorldPoint(topRightCorner);

            SetVariables();

            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.LeftControl) ||
                Input.GetKeyDown(KeyCode.RightControl) ||
                Input.GetButtonDown("Fire1"))
            {
                Blink();
            }

            var pos = transform.position + (Movement * Strafespeed * Time.deltaTime);
            transform.position = GetMoveHere(pos);
        }

        #endregion
    }
}