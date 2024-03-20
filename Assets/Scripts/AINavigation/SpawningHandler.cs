using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningHandler : MonoBehaviour {
    public const float CHECK_SPAWN_WAIT = 0.1f;
    public float CurrentMinEnemies;
    public Transform PlayerTransform;
    public List<GameObject> ActiveEnemies;
    public float ThreatScore;
    void Start() {
        if (PlayerTransform is null) {
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (PlayerTransform is null) {
                throw new System.Exception("No Target, or Player for Spawner Handler on " + this.gameObject.name);
            }
        }
        CurrentMinEnemies = 15;
        StartCoroutine(SpawnCountdown());
    }
    IEnumerator SpawnCountdown() {
        /*
            


        */
        if (ActiveEnemies.Count > CurrentMinEnemies) {
            //int spawnAmount = Random.Range(2, 8)
            //for (int i = 0; i < ActiveEnemies.Count;)
        }
        yield return new WaitForSeconds(CHECK_SPAWN_WAIT);
        StartCoroutine(SpawnCountdown());
    }
}
