using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class melee : MonoBehaviour
{
    public GameObject RollingPin;

    // Start is called before the first frame update
    void Start()
    {
        InputPoller.instance.Input.PlayerCharacacter.Attack.performed -= Swing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Swing(InputAction.CallbackContext context)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        IHealth health = other.GetComponent<IHealth>();
        if (health is not null)
        {
            Debug.Log("Hit" + other.gameObject.name);
            health.DoDamage(2f);
            
        }
    }
}
