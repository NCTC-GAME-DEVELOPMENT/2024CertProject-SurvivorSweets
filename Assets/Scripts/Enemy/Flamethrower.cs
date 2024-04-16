using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flamethrower : MonoBehaviour
{
    [SerializeField]
    private GameObject FireObject;
    public GameObject FireFromPoint;
    private bool isActive;
    void Start()
    {
        InputPoller.instance.Input.PlayerCharacacter.Attack.started += SpewFire;
        InputPoller.instance.Input.PlayerCharacacter.Attack.canceled += StopSpewFire;

    }
    private void OnDestroy() {
        InputPoller.instance.Input.PlayerCharacacter.Attack.started -= SpewFire;
        InputPoller.instance.Input.PlayerCharacacter.Attack.canceled -= StopSpewFire;
    }

    public void SpewFire(InputAction.CallbackContext context) {
        StartCoroutine(FireCoroutine());
        isActive = true;
    }
    public void StopSpewFire(InputAction.CallbackContext context) {
       isActive = false;
    }
    public IEnumerator FireCoroutine() {
        while (isActive) {
            Instantiate(FireObject, FireFromPoint.transform.position, FireFromPoint.transform.rotation);
            yield return new WaitForSeconds(.25f);
            
        }
    }
}
