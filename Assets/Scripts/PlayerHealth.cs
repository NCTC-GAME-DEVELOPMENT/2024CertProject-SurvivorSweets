using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth {
    [SerializeField]
    private float Health;
    public void DoDamage(float value) {
        Health -= value;
        if (Health <= 0) {
            Debug.Log("Player Be dead");
        }
    }

    public void DoDamageOverTime(float value, float timeInSeconds) {
        throw new NotImplementedException();
    }

    public void DoHeal(float value) {
        Health += value;
    }

    public float GetHealth() {
        return Health;
    }

    public Action GetOnDeathEvent() {
        throw new NotImplementedException();
    }

    public bool IsAlive() {
        throw new NotImplementedException();
    }

    public void SetHealth(float value) {
        throw new NotImplementedException();
    }

    public void SubscribeToOnDeath(Action method) {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
