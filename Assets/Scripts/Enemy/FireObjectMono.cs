using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class FireObjectMono : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3f;
    Rigidbody rb;
    public void Start() {
        Destroy(gameObject, lifeTime);
    }
    public void OnParticleCollision(GameObject other) {
        Debug.Log("Fire Damage");
        IHealth health = other.GetComponent<IHealth>();
        if (health is not null)
            health.DoDamageOverTime(2, 4);
        
    }
}
