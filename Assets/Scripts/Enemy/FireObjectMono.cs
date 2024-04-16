using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjectMono : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float lifeTime = 3f;
    Rigidbody rb;
    public void Start() {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        IHealth health = collision.GetComponent<IHealth>();
        if (health is not null)
            health.DoDamageOverTime(2, 4);
    }
}
