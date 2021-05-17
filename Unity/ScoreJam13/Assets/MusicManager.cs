namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;

    public class MusicManager : BehaviourBase
    {
        AudioSource musicsource;
        public AudioClip menumusic;
        public AudioClip gamemusic;

        protected override void Awake()
        {
            base
                .Awake();

            var other = FindObjectOfType<MusicManager>();
            if (other != this)
            {
                DestroyComponent(this);
                return;
            }

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            musicsource = GetComponent<AudioSource>();
            musicsource.clip = menumusic;
            musicsource.Play();
        }

        public void GameStarted()
        {
            musicsource.Stop();
            musicsource.clip = gamemusic;
            musicsource.Play();
        }

        public void GameEnded()
        {
            musicsource.Stop();
            musicsource.clip = menumusic;
            musicsource.Play();
        }
    }
}