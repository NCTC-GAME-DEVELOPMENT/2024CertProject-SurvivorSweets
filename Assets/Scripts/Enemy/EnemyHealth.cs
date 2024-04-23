using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth {
    [SerializeField]
    private float health;
    public Action OnDeathEvent;
    public void DoDamage(float value) {
        health -= value;
        if (health <= 0) {
            OnDeathEvent?.Invoke();
        }
    }

    public void DoDamageOverTime(float value, float timeInSeconds) {
        StartCoroutine(DamageOverTime(value / (timeInSeconds * 5), timeInSeconds * 5));
    }
    public IEnumerator DamageOverTime(float tickDamage, float timeInTicks) {
        for (int i = 0; i < timeInTicks * 5; i++) {
            DoDamage(tickDamage);
            yield return new WaitForSeconds(0.2f);
        }
        Debug.Log("Damage Over Time");
    }

    public void DoHeal(float value) {
        health += value;
    }

    public float GetHealth() {
        return health;
    }

    public void SetHealth(float value) {
        health = value;
    }
    public bool IsAlive() {
        return health <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Action GetOnDeathEvent() {
        return OnDeathEvent;
    }

    public void SubscribeToOnDeath(Action method) {
        OnDeathEvent += method;
    }
    public void OnDestroy() {
        StopAllCoroutines();
    }
}
