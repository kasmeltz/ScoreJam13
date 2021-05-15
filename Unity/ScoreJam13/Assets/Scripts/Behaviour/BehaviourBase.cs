namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    public class BehaviourBase : MonoBehaviour
    {
        protected AudioManager AudioManager { get; set; }

        protected virtual void Awake()
        {
            AudioManager = FindObjectOfType<AudioManager>();
        }
    }
}