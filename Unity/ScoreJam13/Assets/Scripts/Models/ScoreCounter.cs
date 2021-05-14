using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreCounter : MonoBehaviour
{
    public float score;
    public float scoretoadd;
    [SerializeField] Text scoretxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoretoadd;
        int roundedscore = Mathf.RoundToInt(score);
        scoretxt.text = roundedscore.ToString();
    }
}
