namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/DisappearingFloor")]
    public class DisappearingFloorBehaviour : BehaviourBase
    {
        #region Members

        public Animator Animator;

        public float OpenChance;

        public bool IsOpen { get; set; }

        protected bool IsOpening { get; set; }

        protected int Level { get; set; }

        #endregion

        #region Animation Callbacks

        public void Open()
        {
            IsOpen = true;
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            var scoreCounter = FindObjectOfType<ScoreCounter>();

            Level = scoreCounter.Level;

            Level = Mathf.Min(Level, 7);
            
            Animator
                .SetTrigger($"Idle{Level}");
        }

        protected void Update()
        {
            if (IsOpening)
            {
                return;
            }

            if (Random.value <= OpenChance)
            {
                IsOpening = true;

                Animator
                    .SetTrigger($"Sliding{Level}");
            }
        }

        #endregion
    }
}