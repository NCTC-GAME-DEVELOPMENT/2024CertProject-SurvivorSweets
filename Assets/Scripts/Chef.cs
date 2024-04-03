using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chef : MonoBehaviour
{
    public List<GameObject> WeaponList;
    public int WeaponIndex = 0;
    public float MoveSpeed = 10;
    public float rotationRate = 100;
    public GameObject CurrentWeapon;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        InputPoller.instance.InputMovement += Movement;

        rb = GetComponent<Rigidbody>();
        InputPoller.instance.Input.PlayerCharacacter.WeaponNext.performed += NextWeapon;
        InputPoller.instance.Input.PlayerCharacacter.WeaponPrevious.performed += PrevWeapon;
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

    public void NextWeapon(InputAction.CallbackContext context)
    {
        WeaponIndex++;
        if (WeaponIndex >= WeaponList.Count)
        {
            WeaponIndex = 0;
        }

        Destroy(CurrentWeapon);

        CurrentWeapon = GameObject.Instantiate(WeaponList[WeaponIndex],this.transform);




    }
    public void PrevWeapon(InputAction.CallbackContext context)
    {
        WeaponIndex--;
        if (WeaponIndex < 0)
        {
            WeaponIndex = (WeaponList.Count - 1);
        }

        

        Destroy(CurrentWeapon);

        CurrentWeapon = GameObject.Instantiate(WeaponList[WeaponIndex], this.transform);
    }

}
