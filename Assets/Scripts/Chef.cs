using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float rotationRate = 100;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        InputPoller.instance.InputMovement += Movement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Movement(Vector2 input)
    {
        gameObject.transform.Rotate(Vector3.up * (rotationRate * input.x * Time.deltaTime));
    

    
        float usevalue = input.y;
        if (usevalue < 0)
        {
            usevalue *= .5f;
        }

        if (rb)
        {
            rb.velocity = gameObject.transform.forward * (MoveSpeed * usevalue);
        }

    }
}
