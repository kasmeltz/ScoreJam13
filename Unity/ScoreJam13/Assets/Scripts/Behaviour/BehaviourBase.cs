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

        protected void DestroyComponent(Component component)
        {
            if (component == null)
            {
                return;
            }

#if UNITY_EDITOR
            component
                .gameObject
                .SetActive(false);
#endif

            Destroy(component.gameObject);
        }
    }
}