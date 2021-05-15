namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/FloatingLaserSpawner")]
    public class FloatingLaserSpawnerBehaviour : BehaviourBase
    {
        #region Members

        public FloatingLaserBehaviour FloatingLaserPrefab;

        public float SpawnFrequency;

        protected float SpawnCounter { get; set; }

        #endregion

        #region Protected Methods

        #endregion

        #region Unity

        protected void Reset()
        {
            
        }

        protected void Update()
        {
            
        }

        #endregion
    }
}