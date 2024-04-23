using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRemoveAfterTime : MonoBehaviour {
    [SerializeField]
    private float timeTillDestory;
    void Start() {
        Destroy(gameObject, timeTillDestory);
    }

}
