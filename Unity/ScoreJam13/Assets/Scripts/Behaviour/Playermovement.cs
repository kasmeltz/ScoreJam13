using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float BlinkDistance;
    public float Strafespeed;
    Vector2 movement;
    Vector2 pos;
    public Vector3 blink;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetVariables();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Blink();
        }
    }

    

    private void FixedUpdate()
    {
        Normalmovement();
    }
    void Normalmovement()
    {
        transform.Translate(pos + movement * Strafespeed * Time.fixedDeltaTime);
        
    }
    void Blink()
    {
        transform.position += blink;
    }
    private void SetVariables()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x > 0)
        {
            blink = new Vector3(BlinkDistance, 0, 0);
        }
        if (movement.x < 0)
        {
            blink = new Vector3(-BlinkDistance, 0, 0);
        }
        if (movement.y > 0)
        {
            blink = new Vector3(0, BlinkDistance, 0);
        }
        if (movement.y < 0)
        {
            blink = new Vector3(0, -BlinkDistance, 0);
        }
    }
}
