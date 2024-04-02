using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigation {
    public Action<float> GetOnDistanceUpdate();
    public void SubscribeToOnDistanceUpdate(Action<float> method);
}
