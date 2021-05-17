using UnityEngine;

public class Transitions : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowGlobal()
    {
        anim.SetTrigger("Global");
      
    }

    public void ShowPersonal()
    {
        anim.SetTrigger("Personal");
    }
}