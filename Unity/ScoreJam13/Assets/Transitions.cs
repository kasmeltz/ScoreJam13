using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void SwitchG()
    {
        anim.SetBool("Global", true);
      
    }
    public void SwitchV()
    {
        anim.SetBool("Global", false);
        
    }
}
