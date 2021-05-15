using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace KasJam.ScoreJam13.Unity.Behaviours {
    public class Mainmenu : MonoBehaviour
    {
        
        // Start is called before the first frame update
        void Start()
        {
            FindObjectOfType<ScrollingMapGeneratorBehvaiour>().Reset();
        }

        public void Loadscene(int scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
