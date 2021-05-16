namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/ImmunePowerup")]
    public class ImmunePowerupBehaviour : PowerupBehaviourBase
    {
        #region PickupBehaviourBase

        protected override void DoPickedUp()
        {
            Player
                .SetImmune();
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