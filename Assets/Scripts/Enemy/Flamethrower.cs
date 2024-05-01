using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flamethrower : MonoBehaviour
{
    [SerializeField]
    private GameObject FireObject;
    public GameObject FireFromPoint;
    public GameObject FireCollider;
    public AudioBite FireSound;
    private bool isActive;
    void Start()
    {
        FireSound.Init(gameObject);
        InputPoller.instance.Input.PlayerCharacacter.Attack.started += SpewFire;
        InputPoller.instance.Input.PlayerCharacacter.Attack.canceled += StopSpewFire;
        FireCollider.SetActive(false);
    }
    private void OnDestroy() {
        InputPoller.instance.Input.PlayerCharacacter.Attack.started -= SpewFire;
        InputPoller.instance.Input.PlayerCharacacter.Attack.canceled -= StopSpewFire;
    }

    public void SpewFire(InputAction.CallbackContext context) {
        isActive = true;
        StartCoroutine(FireCoroutine());
        StartCoroutine(SoundCoroutine());
    }
    public void StopSpewFire(InputAction.CallbackContext context) {
       isActive = false;
    }
    public IEnumerator FireCoroutine() {
        while (isActive) {
            Instantiate(FireObject, FireFromPoint.transform.position, FireFromPoint.transform.rotation, this.transform);
            FireCollider.SetActive(true);
            yield return new WaitForSeconds(.25f);
            
        }
        FireCollider.SetActive(false);
    }
    public IEnumerator SoundCoroutine() {
        while (isActive) {
            FireSound.Play();
            yield return new WaitForSeconds(FireSound.GetClipLength());
        }
    }

}
