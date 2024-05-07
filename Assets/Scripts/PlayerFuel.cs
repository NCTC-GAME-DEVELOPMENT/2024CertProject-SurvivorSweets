using Hiyazcool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFuel : MonoBehaviour {
    [SerializeField]
    private int maxFuel;
    [SerializeField]
    private int fuel;
    public event Action OnFuelUpdate;
    [SerializeField]
    private Slider slider;

    public void Start() {
        //SceneFuelger.sceneLoaded += OnSceneLoad;
        OnFuelUpdate += FuelPrivateUpdate;
        slider.maxValue = maxFuel;
        StartCoroutine(PassiveRegen());
    }

    private void FuelPrivateUpdate() {
        slider.value = fuel;
    }

    public bool UseFuel(int _cost) {
        Debug.Log(_cost);
        if (fuel - _cost > -1) {
            fuel -= _cost;
            OnFuelUpdate?.Invoke();
            StopCoroutine(PassiveRegen());
            
            return true;
        } else {
            
            return false;
        }
    }
    public int GetFuel() => fuel;
    public int GetMaxFuel() => maxFuel;
    public float GetFuelFill() {
        return fuel / maxFuel;
    }
    public IEnumerator PassiveRegen() {
        while (true) {
            RegenFuel(1);
            yield return new WaitForFixedUpdate();
        }
        
    }
    public void StartRegen(float time) => StartCoroutine(StartPrivateRegen(time));
    public IEnumerator StartPrivateRegen(float timeTillStart) {
        yield return new WaitForSeconds(timeTillStart);
        StartCoroutine(PassiveRegen());

    }
    public void RegenFuel(int _amount) {
        OnFuelUpdate?.Invoke();
        fuel += _amount;
        if (fuel > maxFuel) {
            fuel = maxFuel;
        }
    }

}
