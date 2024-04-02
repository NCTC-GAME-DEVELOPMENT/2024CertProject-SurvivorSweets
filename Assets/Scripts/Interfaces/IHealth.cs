using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth {
    public void DoDamage(float value);
    public void DoHeal(float value);
    public float GetHealth();
    /// <summary>
    /// Do a certain amount of damage of a certain amout of time
    /// </summary>
    /// <param name="value">Total Damage to be done</param>
    /// <param name="timeInSeconds">Time to have damage span over in seconds</param>
    public void DoDamageOverTime(float value, float timeInSeconds);
    public void SetHealth(float value);
    public bool IsAlive();
    public Action GetOnDeathEvent();
}
