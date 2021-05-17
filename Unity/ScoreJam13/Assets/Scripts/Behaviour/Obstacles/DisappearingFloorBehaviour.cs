namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/DisappearingFloor")]
    public class DisappearingFloorBehaviour : BehaviourBase
    {
        #region Members

        public Animator Animator;

        public float OpenChance { get; set; }
    
        public bool IsOpen { get; set; }

        protected bool IsOpening { get; set; }

        #endregion

        #region Animation Callbacks

        public void Open()
        {
            IsOpen = true;
        }

        #endregion

        #region Unity

        protected void Update()
        {
            if (IsOpening)
            {
                return;
            }

            if (Random.value >= OpenChance)
            {
                IsOpening = true;

                Animator
                    .SetTrigger("Sliding");
            }
        }

        #endregion
    }
}