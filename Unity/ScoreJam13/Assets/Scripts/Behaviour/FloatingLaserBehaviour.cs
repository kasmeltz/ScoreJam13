namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/FloatingLaser")]
    public class FloatingLaserBehaviour : BehaviourBase
    {
        #region Members

        public LaserBehaviour LaserPrefab;

        #endregion

        #region Animation Callbacks

        public void SpawnLaser()
        {
            var laser = Instantiate(LaserPrefab);

            laser.transform.position = Vector3.zero;
            laser.transform.SetParent(transform.parent);
        }

        public void SelfDestruct()
        {
            DestroyComponent(this);
        }

        #endregion
    }
}