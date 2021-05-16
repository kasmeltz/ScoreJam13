namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BlinkPowerUp : MonoBehaviour
    {
        public PlayermovementBase player;
        public bool test;

        private void Update()
        {
            if(test)
            {
                RandomBlink();
                test = false;
            }
        }

        protected void RandomBlink()
        {

            print("blink");
            int dir = Random.Range(0, 4);

            player.Blink();

            if(dir == 0)
            {
                player.BlinkVector = new Vector3(1, 0, 0) * player.BlinkDistance;
            }

            if (dir == 1)
            {
                player.BlinkVector = new Vector3(-1, 0, 0) * player.BlinkDistance;
            }

            if (dir == 2)
            {
                player.BlinkVector = new Vector3(0, 1, 0) * player.BlinkDistance;
            }

            if (dir == 3)
            {
                player.BlinkVector = new Vector3(0, -1, 0) * player.BlinkDistance;
            }

            player.Blink();

        }
    }
}
