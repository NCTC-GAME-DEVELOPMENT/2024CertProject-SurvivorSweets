using Hiyazcool.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviourSingleton<EnemySpawner> {
    private float threatScore;
    private float currentScore;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private List<SpawnRates> spawnRates;
    private List<EnemyController> enemies;
    private List<EnemyController> inactiveEnemies;
    private float TotalWeight;
    [SerializeField]
    private LayerMask layerMask;
    
    void Start() {
        Random.InitState((int) DateTime.Now.Ticks);
        threatScore = 30;
        currentScore = 0;
        foreach (SpawnRates rates in spawnRates) {
            TotalWeight += rates.weight;
        }
        StartCoroutine(SpawnLoop());
    }
    void Update() {

    }
    private IEnumerator SpawnLoop() {
        Debug.Log("SpawnLoop");
        
        if (currentScore < threatScore) {
            int ran = Random.Range(4,8);
            //if (inactiveEnemies.Count > ran) {

            //} else
                Spawn(ran);
            
        }
        Random.InitState((int) DateTime.Now.Ticks);
        yield return new WaitForSeconds(1);
        SpawnLoop();
    }
    public void RemoveFromCurrentList(EnemyController enemyController) {
        inactiveEnemies.Add(enemyController);
        enemies.Remove(enemyController);
        enemyController.gameObject.SetActive(false);
        currentScore -= enemyController.threatScore;
        threatScore += enemyController.threatScore;
    }
    public void Spawn(int num) {
        for (int i = 0; i < num; i++) {
            GameObject.Instantiate(GetSpawnObject(), GetSpawnPoint(),Quaternion.identity, this.transform);
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
            1,
            Random.Range(10,25)
            );
        if (GetRandomBool()) 
            spawnPoint.x *= -1;
        Debug.Log(spawnPoint);
        if (GetRandomBool())
            spawnPoint.z *= -1;
        if (Physics.Raycast(spawnPoint, Vector3.down, 20, layerMask))
            return spawnPoint;
        Debug.Log("Woo");

        return GetSpawnPoint();
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
