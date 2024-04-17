using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float MoveSpeed = 15f;
    public float LifeTime = 5f;


    Rigidbody rb;



    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = gameObject.transform.forward * MoveSpeed;
        Destroy(gameObject, LifeTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
            return;
        IHealth health = other.GetComponent<IHealth>();
        if (health is not null) {
            Debug.Log("Hit" + other.gameObject.name);
            health.DoDamage(2f);
            Destroy(gameObject);
        }
    }

}
