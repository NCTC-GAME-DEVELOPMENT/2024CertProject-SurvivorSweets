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
        isActive = true;
        StartCoroutine(FireCoroutine());
    }
    public void StopSpewFire(InputAction.CallbackContext context) {
       isActive = false;
    }
    public IEnumerator FireCoroutine() {
        Debug.Log("Gotta commit Warcrimess");
        while (isActive) {
            Instantiate(FireObject, FireFromPoint.transform.position, FireFromPoint.transform.rotation, this.transform);
            yield return new WaitForSeconds(.25f);
            
        }
    }
}
