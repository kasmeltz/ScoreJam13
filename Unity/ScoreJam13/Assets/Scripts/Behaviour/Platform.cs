using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private float _speed = 3;
    private float _endposx = 10;


    public void startFloating(float speed, float endposx)
    {
        _speed = speed;
        _endposx = endposx;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _speed);

        if (transform.position.x > _endposx)
        {
            Destroy(gameObject);
        }


    }
}
