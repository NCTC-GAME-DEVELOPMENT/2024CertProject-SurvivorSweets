using Hiyazcool.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviourSingleton<EnemySpawner> {
    [SerializeField]
    private float threatScore;
    [SerializeField]
    private float currentScore;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private List<SpawnRates> spawnRates;
    private float TotalWeight;
    [SerializeField]
    private LayerMask layerMask;
    
    void Start() {
        Random.InitState((int) DateTime.Now.Ticks);
        threatScore = 5;
        currentScore = 0;
        foreach (SpawnRates rates in spawnRates) {
            TotalWeight += rates.weight;
        }
        StartCoroutine(SpawnLoop());
    }
    void Update() {

    }
    public IEnumerator SpawnLoop() {
        
        if (currentScore < threatScore) {
            int ran = Random.Range(4,8);
            //if (inactiveEnemies.Count > ran) {

            //} else
                Spawn(ran);
            
        }
        Random.InitState((int) DateTime.Now.Ticks);
        yield return new WaitForSeconds(1);
        Debug.Log("SpawnLoop");
        StartCoroutine(SpawnLoop());
    }
    public void RemoveFromCurrentList(float _threatScore) {
        
        currentScore -= _threatScore;
        threatScore += _threatScore;
        
    }
    public void Spawn(int num) {
        for (int i = 0; i < num; i++) {
            Random.InitState((int) Time.deltaTime + i);
            Vector3 spawn = GetSpawnPoint();
            if (spawn == Vector3.zero) {
                spawn = GetSpawnPoint();
                if (spawn == Vector3.zero)
                    continue;
            }
            GameObject enemy = GameObject.Instantiate(GetSpawnObject(), spawn ,Quaternion.identity, this.transform);
            currentScore += enemy.GetComponent<EnemyController>().threatScore;
        }
    }
    private GameObject GetSpawnObject() {
        float weight = 0;
        float ranNum = Random.Range(0,TotalWeight);
        foreach (SpawnRates rates in spawnRates) {
            weight += rates.weight;
            if (weight >= ranNum) {
                return rates.spawn;
            }
        }
        return spawnRates[0].spawn;
    }
    public Vector3 GetSpawnPoint() {
        
        Vector3 spawnPoint = new Vector3(
            Random.Range(10,25),
            10,
            Random.Range(10,25)
            );
        if (GetRandomBool()) 
            spawnPoint.x *= -1;
        //Debug.Log(spawnPoint);
        if (GetRandomBool())
            spawnPoint.z *= -1;
        spawnPoint += playerTransform.position;
        if (Physics.Raycast(spawnPoint, Vector3.down, 20, layerMask)) {
            spawnPoint.y = 1;
            return spawnPoint;
        }
        //Debug.Log("Woo");

        return Vector3.zero;
    }
    public bool GetRandomBool() {
        int num = Random.Range(0, 10);
        if (num >= 6)
            return false;
        return true;
    }
    public void SpawnFromInactive() {
        
    }

    [Serializable]
    private class SpawnRates {
        public float weight;
        public float threatScore;
        public GameObject spawn;
    }
}
