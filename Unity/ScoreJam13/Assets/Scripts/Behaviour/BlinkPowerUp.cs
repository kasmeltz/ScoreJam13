namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/BlinkTile")]
    public class BlinkPowerUp : BehaviourBase
    {        
        public Vector2 Direction;

        protected Playermovement Player { get; set; }

        #region Unity

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            Blink();
        }

        protected override void Awake()
        {
            base
                .Awake();

            Player = FindObjectOfType<Playermovement>();
        }

        #endregion

        protected void Blink()
        {
            Player.BlinkVector = Direction * Player.BlinkDistance;
            Player.Blink();
        }
    }
}
