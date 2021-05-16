namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    public abstract class PowerupBehaviourBase : BehaviourBase
    {
        #region Members

        public PowerUpType PowerUpType;

        public float TimeToLive;

        protected float TimeAlive { get; set; }

        public bool IsPickedUp { get; protected set; }

        public Playermovement Player { get; set; }

        #endregion

        #region Events

        public event EventHandler TimeExpired;

        protected void OnTimeExpired()
        {
            TimeExpired?
                .Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        public void PickUp()
        {
            IsPickedUp = true;
            transform.localScale = new Vector3(0, 0, 0);

            DoPickedUp();
        }        

        public void Die()
        {
            DestroyComponent(this);
        }

        public abstract void DoWhenDestroyed();

        public abstract void DoUpdate();

        #endregion

        #region Protected Methods

        protected abstract void DoPickedUp();

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            TimeAlive = 0;
            IsPickedUp = false;
        }

        protected virtual void Update()
        {
            if (!IsPickedUp)
            {
                return;
            }

            TimeAlive += Time.unscaledDeltaTime;
            if (TimeAlive >= TimeToLive)
            {
                DoWhenDestroyed();

                OnTimeExpired();

                DestroyComponent(this);

                return;
            }

            DoUpdate();
        }

        #endregion
    }
}