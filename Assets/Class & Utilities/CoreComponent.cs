using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public abstract class CoreComponent
{
    [SerializeField, HideInInspector]
    private string _name;
    public void SetComponentName() => _name = GetType().Name;
    public CoreComponent() {
        SetComponentName();
    }
    public virtual void OnDrawGizmos(Transform transform) {
    }
}
