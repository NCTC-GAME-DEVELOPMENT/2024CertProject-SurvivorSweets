using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StraightTrackingAI : MonoBehaviour {
    // If I have Enough Time I want to Automate increase/decreasing according to game frame rate
    public const float CHECK_TARGET_WAIT = 0.1f;
    public Transform TargetTransform;
    [HideInInspector]
    public NavMeshAgent Agent;
    void Start() {
        Agent = GetComponent<NavMeshAgent>();
        if (TargetTransform is null) {
            TargetTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (TargetTransform is null) {
                throw new System.Exception("No Target, or Player for " + this.gameObject.name);
            }
        }
        StartCoroutine(CheckTarget());
    }
    public IEnumerator CheckTarget() {
        yield return new WaitForSeconds(CHECK_TARGET_WAIT);
        Agent.SetDestination(TargetTransform.position);
        StartCoroutine(CheckTarget());
    }
}
