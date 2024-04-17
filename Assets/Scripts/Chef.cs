using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chef : MonoBehaviour, IHealth {
    public List<GameObject> WeaponList;
    public int WeaponIndex = 0;
    public float MoveSpeed = 10;
    public float rotationRate = 100;
    public GameObject CurrentWeapon;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        CurrentWeapon = GameObject.Instantiate(WeaponList[WeaponIndex], this.transform);
        InputPoller.instance.InputMovement += Movement;

        rb = GetComponent<Rigidbody>();
        InputPoller.instance.Input.PlayerCharacacter.WeaponNext.performed += NextWeapon;
        InputPoller.instance.Input.PlayerCharacacter.WeaponPrevious.performed += PrevWeapon;

    }

    // Update is called once per frame
    void Update() {

    }
    public void Movement(Vector2 input) {
        gameObject.transform.Rotate(Vector3.up * (rotationRate * input.x * Time.deltaTime));



        float usevalue = input.y;
        if (usevalue < 0) {
            usevalue *= .5f;
        }

        if (rb) {
            rb.velocity = gameObject.transform.forward * (MoveSpeed * usevalue);
        }

    }

    public void NextWeapon(InputAction.CallbackContext context) {
        WeaponIndex++;
        if (WeaponIndex >= WeaponList.Count) {
            WeaponIndex = 0;
        }
        if (CurrentWeapon is not null)
            Destroy(CurrentWeapon);

        CurrentWeapon = GameObject.Instantiate(WeaponList[WeaponIndex], this.transform);




    }
    public void PrevWeapon(InputAction.CallbackContext context) {
        WeaponIndex--;
        if (WeaponIndex < 0) {
            WeaponIndex = (WeaponList.Count - 1);
        }


        if (CurrentWeapon is not null)
            Destroy(CurrentWeapon);

        CurrentWeapon = GameObject.Instantiate(WeaponList[WeaponIndex], this.transform);
    }

    #region Health 
    [SerializeField]
    private float health = 20;
    public void DoDamage(float value) {
        health -= value;
        CheckHealth();
    }

    private void CheckHealth() {
        if (health >= 0)
            return;
        Destroy(gameObject);
        Destroy(CurrentWeapon);
    }

    public void DoHeal(float value) {
        health += value;
    }

    public float GetHealth() {
        return health;
    }

    public void DoDamageOverTime(float value, float timeInSeconds) {
        throw new NotImplementedException();
    }

    public void SetHealth(float value) {
        health = value;
    }

    public bool IsAlive() {
        return health >= 0;
    }

    public Action GetOnDeathEvent() {
        throw new NotImplementedException();
    }

    public void SubscribeToOnDeath(Action method) {
        throw new NotImplementedException();
    }
    #endregion
}
