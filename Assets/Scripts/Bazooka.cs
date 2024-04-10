using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bazooka : MonoBehaviour
{
    public GameObject FireFromPoint;
    public GameObject projectilePrefab;



    // Start is called before the first frame update
    void Start()
    {
        
        InputPoller.instance.Input.PlayerCharacacter.Attack.performed += FireMarsh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        InputPoller.instance.Input.PlayerCharacacter.Attack.performed -= FireMarsh;
    }
    public void FireMarsh(InputAction.CallbackContext context)
    {
        Debug.Log(projectilePrefab);
        Debug.Log(FireFromPoint.transform.rotation);
        Debug.Log(FireFromPoint.transform.position);
        Instantiate(projectilePrefab, FireFromPoint.transform.position, FireFromPoint.transform.rotation);
    }
    
}
