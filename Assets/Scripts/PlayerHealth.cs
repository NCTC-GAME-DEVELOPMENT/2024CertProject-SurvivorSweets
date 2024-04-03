using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public void DoDamage(float value) {
        Debug.Log("Did Damage to player");
    }

    public void DoDamageOverTime(float value, float timeInSeconds) {
        throw new NotImplementedException();
    }

    public void DoHeal(float value) {
        throw new NotImplementedException();
    }

    public float GetHealth() {
        throw new NotImplementedException();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
