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
        anim.SetTrigger("Global");
      
    }
    public void SwitchV()
    {
        anim.SetTrigger("Personal");    
    }
}
