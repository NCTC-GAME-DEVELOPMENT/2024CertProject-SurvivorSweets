using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PWPawn
{

    
    public float MoveSpeed = 10;
    public float rotationRate = 100;


    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Horizontal(float value)
    {
        gameObject.transform.Rotate(Vector3.up * (rotationRate * value * Time.deltaTime));
    }

    public override void Vertical(float value)
    {
        float usevalue = value;
        if (usevalue < 0)
        {
            usevalue = value * .5f;
        }

        if (rb)
        {
            rb.velocity = gameObject.transform.forward * (MoveSpeed * usevalue);
        }

    }

}
