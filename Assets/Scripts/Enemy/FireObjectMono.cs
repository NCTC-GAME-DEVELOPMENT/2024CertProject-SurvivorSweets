using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class FireObjectMono : MonoBehaviour {
    [SerializeField]
    private Collider fireCollider;
    private bool fireOn;
    public void Start() {
        if ( fireCollider is null ) {
            fireCollider = GetComponent<Collider>();
            if ( fireCollider is null ) {
                throw new System.Exception("No Collider on FireObject, " + this.name);
            }
        }
        fireOn = true;
    }
    public void OnEnable() {
        //StartCoroutine(FireTick());
    }
    public void OnDisable() {
        StopAllCoroutines();
    }
    public void OnTriggerStay(Collider other) {
        if (fireOn) {
            Debug.Log("Fire On");
            if (other.tag == "Enemy") {
                Debug.Log("Enemy " + other.name);
                IHealth health = other.GetComponent<IHealth>();
                if (health != null) {
                    health.DoDamageOverTime(1, 5);
                    Debug.Log("Trying Damage");
                }
            }
        }
    }
    IEnumerator FireTick() {
        fireOn = true;
        yield return new WaitForEndOfFrame();
        fireOn = false;
        yield return new WaitForSeconds(.25f);
        StartCoroutine(FireTick());
    }
}
