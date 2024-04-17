using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameramoveScript : MonoBehaviour {
    [SerializeField]
    private Transform player;
    public void Update() {
        Vector3 loc = transform.position;
        loc.x = player.position.x;
        loc.z = player.position.z;
        transform.position = loc;
    }
}
