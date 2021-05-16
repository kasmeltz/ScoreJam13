namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System;
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/PlayerCharacter")]
    public class Playermovement : PlayermovementBase
    {
        #region Members

        public float CoinScore;

        public static bool IsPlaying { get; set; }

        public static string PlayerName { get; set; }

        #endregion

        #region Protected Methods

        public void Reset()
        {
            IsDead = false;
            transform.position = Vector3.zero;
            BlinkVector = new Vector3(0, 1, 0);
        }

        #endregion

        #region Unity

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            var coin = collision
                .GetComponent<CoinBehaviour>();    

            if (coin != null)
            {
                AudioManager.Playoneshot("Pickup");
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

        protected override void Update()
        {
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