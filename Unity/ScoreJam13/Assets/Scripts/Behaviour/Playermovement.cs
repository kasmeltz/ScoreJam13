namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/PlayerCharacter")]
    public class Playermovement : PlayermovementBase
    {
        #region Members

        public float CoinScore;

        public static string PlayerName { get; set; }
        
        #endregion

        #region Protected Methods

        public void Reset()
        {
            IsDead = false;
            transform.position = Vector3.zero;
            BlinkVector = new Vector3(0, 1, 0);
            ExtraLives = 0;
            UpdateExtraLives();
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
                if (!powerup.IsPickedUp)
                {
                    AudioManager
                        .Playoneshot("Pickup");

                    var powerUpType = powerup.PowerUpType;

                    powerup
                        .PickUp();

                    if (powerUpType != PowerUpType.Slowdown)
                    {
                        DestroyComponent(powerup);
                    }
                }
            }

            var fakeFloor = collision
                .GetComponent<DisappearingFloorBehaviour>();

            if (fakeFloor != null)
            {
                if (fakeFloor.IsOpen)
                {
                    Die();
                    
                    return;
                }
            }               
        }

        protected void OnTriggerStay2D(Collider2D collision)
        {
            var fakeFloor = collision
               .GetComponent<DisappearingFloorBehaviour>();

            if (fakeFloor != null)
            {
                if (fakeFloor.IsOpen)
                {
                    Die();

                    return;
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