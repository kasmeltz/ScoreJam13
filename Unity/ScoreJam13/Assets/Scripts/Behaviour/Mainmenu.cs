using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KasJam.ScoreJam13.Unity.Behaviours {
    public class Mainmenu : MonoBehaviour
    {
        
        // Start is called before the first frame update
        void Start()
        {
            FindObjectOfType<ScrollingMapGeneratorBehvaiour>().Reset();
        }


    }
}
