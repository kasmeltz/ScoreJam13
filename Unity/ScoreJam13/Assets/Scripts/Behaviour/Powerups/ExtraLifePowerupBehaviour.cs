namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/ExtraLifePowerup")]
    public class ExtraLifePowerupBehaviour : PowerupBehaviourBase
    {
        #region PickupBehaviourBase

        protected override void DoPickedUp()
        {
            Player
                .GainLife();
        }

        public override void DoUpdate()
        {
        }

        public override void DoWhenDestroyed()
        {
        }

        #endregion
    }
}