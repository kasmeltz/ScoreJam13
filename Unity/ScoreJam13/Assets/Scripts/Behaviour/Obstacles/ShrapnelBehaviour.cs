namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/Shrapnel")]
    public class ShrapnelBehaviour : BehaviourBase
    {
        #region Members

        public float MoveSpeed;

        public float TimeToLive;

        public Rigidbody2D RigidBody;

        #endregion

        #region Unity

        protected void Update()
        {
            TimeToLive -= Time.deltaTime;
            if (TimeToLive <= 0)
            {
                DestroyComponent(this);
            }
        }

        #endregion
    }
}