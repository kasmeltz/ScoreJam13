namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/Bomb")]
    public class BombBehaviour : BehaviourBase
    {
        #region Members

        public ShrapnelBehaviour ShrapnelPrefab;

        public float MinShrapnelChange;

        public float MaxShrapnelChange;

        #endregion

        #region Animation Callbacks

        public void Explode()
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    var shrapnel = Instantiate(ShrapnelPrefab);
                    shrapnel.transform.SetParent(transform.parent);
                    shrapnel.transform.position = transform.position;

                    shrapnel
                        .GetComponent<Rigidbody2D>();

                    float fx = x * Random.Range(MinShrapnelChange, MaxShrapnelChange);
                    float fy = y * Random.Range(MinShrapnelChange, MaxShrapnelChange);

                    shrapnel.RigidBody.velocity = new Vector3(fx, fy, 0) * shrapnel.MoveSpeed;                        
                }
            }

            DestroyComponent(this);
        }

        #endregion

        #region Unity

        protected void Update()
        {
            if (transform.position.y <= 1)
            {
                GetComponent<Animator>()
                    .SetTrigger("Explode");
            }
            else if (transform.position.y <= 3)
            {
                if (Random.value >= 0.95)
                {
                    GetComponent<Animator>()
                        .SetTrigger("Explode");
                }
            }
        }

        #endregion
    }
}