namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/DisappearingFloor")]
    public class DisappearingFloorBehaviour : BehaviourBase
    {
        #region Members

        public bool IsOpen { get; set; }

        #endregion

        #region Animation Callbacks

        public void Open()
        {
            IsOpen = true;
        }

        #endregion

        #region Unity

        #endregion
    }
}