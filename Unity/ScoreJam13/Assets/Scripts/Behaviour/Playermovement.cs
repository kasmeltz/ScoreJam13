namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/PlayerCharacter")]
    public class Playermovement : PlayermovementBase
    {
        #region Members

        public float CoinScore;

        public static string PlayerName { get; set; }
        
        #endregion

        #region Event Handlers

        private void Powerup_TimeExpired(object sender, EventArgs e)
        {
            var powerUp = (PowerupBehaviourBase)sender;

            if (ActivePowerups.ContainsKey(powerUp.PowerUpType))
            {
                ActivePowerups
                    .Remove(powerUp.PowerUpType);
            }
        }

        #endregion

        #region Protected Methods

        public void Reset()
        {
            IsDead = false;
            transform.position = Vector3.zero;
            BlinkVector = new Vector3(0, 1, 0);
            ExtraLives = 0;
            UpdateExtraLives();
            ActivePowerups
                .Clear();
            Collider.size = OldColliderSize;
        }

        #endregion

        #region Unity

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            var coin = collision
                .GetComponent<CoinBehaviour>();    

            if (coin != null)
            {
                AudioManager
                    .Playoneshot("Pickup");

                ScoreCounter.score += CoinScore;

                DestroyComponent(coin);
            }

            var powerup = collision
                .GetComponent<PowerupBehaviourBase>();

            if (powerup != null)
            {
                AudioManager
                    .Playoneshot("Pickup");

                var powerUpType = powerup.PowerUpType;
                if (powerUpType == PowerUpType.Slowdown)
                {
                    if (ActivePowerups
                        .ContainsKey(powerup.PowerUpType))
                    {
                        ActivePowerups[powerup.PowerUpType]
                            .Die();
                    }

                    ActivePowerups[powerup.PowerUpType] = powerup;
                }

                powerup
                    .PickUp();

                if (powerUpType == PowerUpType.Slowdown)
                {
                    powerup.TimeExpired += Powerup_TimeExpired;
                } 
                else
                {
                    DestroyComponent(powerup);
                }
            }            
        }

        protected void OnCollisionStay2D(Collision2D collision)
        {
            if (!IsBlinking)
            {
                var missile = collision.collider
                    .GetComponent<Missile>();

                if (missile != null)
                {
                    DestroyComponent(missile);
                }

                Die();
            }
        }

        protected override void Update()
        {
            Rigidbody.angularVelocity = 0;
            Rigidbody.velocity = Vector3.zero;

            if (!IsPlaying)
            {
                return;
            }

            base
                .Update();
        }

        protected void Start()
        {
            Reset();            
        }

        #endregion
    }
}