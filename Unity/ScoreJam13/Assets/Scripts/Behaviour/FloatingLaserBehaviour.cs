namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/FloatingLaser")]
    public class FloatingLaserBehaviour : BehaviourBase
    {
        #region Members

        public LaserBehaviour LaserPrefab;

        protected Playermovement Player { get; set; }

        public bool Rotating { get; set; }

        #endregion

        #region Animation Callbacks

        public void SpawnLaser()
        {
            var laser = Instantiate(LaserPrefab);

            laser.transform.SetParent(transform);
            laser.transform.position = transform.position;
            laser.transform.eulerAngles = transform.eulerAngles;
        }

        public void StopRotating()
        {
            Rotating = false;
        }

        public void SelfDestruct()
        {
            DestroyComponent(this);
        }

        public void Playlasersound()
        {
            AudioManager.Playoneshot("Lazer");
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Player = FindObjectOfType<Playermovement>();
            transform.eulerAngles = new Vector3(0, 0, 0);            
        }

        protected void Update()
        {
            if (Rotating)
            {
                float angle = Vector2
                    .Angle(Vector2.right, Player.transform.position - transform.position) * -1.0f;

                transform.eulerAngles = new Vector3(0, 0, angle + 90);
            }
        }

        #endregion
    }
}