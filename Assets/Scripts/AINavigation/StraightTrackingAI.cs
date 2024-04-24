using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StraightTrackingAI : MonoBehaviour, INavigation {
    // If I have Enough Time I want to Automate increase/decreasing according to game frame rate
    public const float CHECK_TARGET_WAIT = 0.1f;
    public Transform TargetTransform;
    [HideInInspector]
    public NavMeshAgent Agent;
    public Action<float> OnDistanceUpdate;
    void Start() {
        //OnDistanceUpdate = new Action<float>(OnDistanceUpdate);
        Agent = GetComponent<NavMeshAgent>();
        
            TargetTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log("Finding Player");
            if (TargetTransform is null) {
                throw new System.Exception("No Target, or Player for " + this.gameObject.name);
            }
        
        Agent.stoppingDistance = 3f;
        StartCoroutine(CheckTarget());
    }
    public IEnumerator CheckTarget() {
        if (TargetTransform is not null) {
            
        Agent.SetDestination(TargetTransform.position);
        OnDistanceUpdate?.Invoke(Vector3.Distance(Agent.transform.position, TargetTransform.position));
        }
        yield return new WaitForSeconds(CHECK_TARGET_WAIT);
        StartCoroutine(CheckTarget());
    }

    public Action<float> GetOnDistanceUpdate() {
        return OnDistanceUpdate;
    }

    public void SubscribeToOnDistanceUpdate(Action<float> method) {
        OnDistanceUpdate += method;
    }
}
