namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("AScoreJam13/Coin")]
    public class CoinBehaviour : BehaviourBase
    {
        private void OnCollisionEnter(Collision collision)
        {
            Camera.main.GetComponent<AudioManager>().Playoneshot("Pickup");
        }
    }
}