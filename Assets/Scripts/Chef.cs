using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chef : MonoBehaviour, IHealth {
    public List<GameObject> WeaponList;
    public int WeaponIndex = 0;
    public float MoveSpeed = 10;
    public float rotationRate = 100;
    public GameObject CurrentWeapon;
    [Header("Sounds")]
    public AudioBite HurtSound;
    public AudioBite DeathSound;

    Rigidbody rb;
    public Camera cam;

    // Start is called before the first frame update
    void Start() {
        HurtSound.Init(gameObject);
        DeathSound.Init(gameObject);
        CurrentWeapon = GameObject.Instantiate(WeaponList[WeaponIndex], this.transform);
        InputPoller.instance.InputMovement += Movement;

        rb = GetComponent<Rigidbody>();
        InputPoller.instance.Input.PlayerCharacacter.WeaponNext.performed += NextWeapon;
        InputPoller.instance.Input.PlayerCharacacter.WeaponPrevious.performed += PrevWeapon;

    }

    // Update is called once per frame
    void Update() {
        ////Debug.Log(Input.mousePosition);
        //Vector3 mouseVector = cam.ViewportToWorldPoint (Input.mousePosition);
        //Debug.Log(mouseVector);
        //transform.LookAt(new Vector3(mouseVector.x, this.transform.position.y, mouseVector.z));
        Vector3 mouseVector = Input.mousePosition;
        mouseVector -= cam.WorldToScreenPoint(this.transform.position);
        float angle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        transform.rotation = Quaternion.Euler(0, -angle, 0); 
    }
    public void Movement(Vector2 input) {
        /*  
        gameObject.transform.Rotate(Vector3.up * (rotationRate * input.x * Time.deltaTime));



        float usevalue = input.y;
        if (usevalue < 0) {
            usevalue *= .5f;
        }

        if (rb) {
            rb.velocity = gameObject.transform.forward * (MoveSpeed * usevalue);
        }
        */

        Vector3 movevector = Vector3.zero;
        // Z Global is moving on Screen X Axis 
        // X Global is moving on Screen Y Axis but flipped
        movevector.x = -input.y;
        movevector.z = input.x;
        if (input.magnitude > 0)
        {
            //gameObject.transform.forward = movevector; 
        }

  
        if (rb)
        {
            rb.velocity = (MoveSpeed * movevector);
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

    private void Death()
    {
        if (health <= 0)
        {
            DeathSound.Play();
            SceneManager.LoadScene("GameOver");
        }
      
   
        
    }

    #region Health 
    
    public float health = 20;
    public Slider slider;
    public void DoDamage(float value) {
        SetHealth(health - value);
        if (CheckHealth())
            HurtSound.Play();
        
    }

    private bool CheckHealth() {
        if (health >= 0)
            return true;
        Death();
        Destroy(gameObject);
        Destroy(CurrentWeapon);
        return false;
    }

    public void DoHeal(float value) {
        SetHealth(health + value);
    }

    public float GetHealth() {
        return health;
    }

    public void DoDamageOverTime(float value, float timeInSeconds) {
        throw new NotImplementedException();
    }

    public void SetHealth(float value) {
        health = value;
        slider.value = value;
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
