namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/SlowDownPowerup")]
    public class SlowDownPowerupBehaviour : PowerupBehaviourBase
    {
        #region Members

        public float SlowdownFactor;

        public float DeadzoneTime;

        public float RampInFactor;

        public float RampOutFactor;

        #endregion

        #region PickupBehaviourBase

        protected override void DoPickedUp()
        {
        }

        public override void DoWhenDestroyed()
        {
            Time.timeScale = 1;
            AudioManager.GlobalPitchModifier = 1;
        }

        public override void DoUpdate()
        {
            float midway = (TimeToLive / 2);
            float halfDeadZone = DeadzoneTime / 2;

            // don't do anything in the dead zone time
            if (TimeAlive >= midway - halfDeadZone && TimeAlive <= midway + halfDeadZone)
            {
                return;
            }

            float d;
            float n;
            if (TimeAlive <= midway)
            {
                n = midway - TimeAlive;
                d = (midway - halfDeadZone) * RampInFactor;
            }
            else
            {
                n = TimeAlive - midway;
                d = (midway - halfDeadZone) * RampOutFactor;
            }

            n = Mathf.Min(n, d);

            float ratio = 1;
            if (d != 0)
            {
                ratio = n / d;
            }

            var slowDown = SlowdownFactor * ratio;

            Time.timeScale = slowDown;
            AudioManager.GlobalPitchModifier = slowDown;
        }

        protected void OnDestroy()
        {
            if (IsPickedUp)
            {
                Time.timeScale = 1;
                AudioManager.GlobalPitchModifier = 1;
            }
        }

        #endregion
    }
}