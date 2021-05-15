namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/Laser")]
    public class LaserBehaviour : BehaviourBase
    {
        #region Animation Callbacks

        public void SelfDestruct()
        {
            DestroyComponent(this);
        }

        #endregion
    }
}