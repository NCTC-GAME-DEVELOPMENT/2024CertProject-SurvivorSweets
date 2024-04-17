using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private IHealth healthComponent;
    private INavigation navigationComponent;
    private IAttack attackComponent;
    public float threatScore;
    public void Start() {
        healthComponent = GetComponent<IHealth>();
        navigationComponent = GetComponent<INavigation>();
        attackComponent = GetComponent<IAttack>();

        bool[] CheckListArray = new bool[3];
        if (healthComponent is null)
            CheckListArray[0] = false;
        else
            CheckListArray[0] = true;
        if (navigationComponent is null)
            CheckListArray[1] = false;
        else
            CheckListArray[1] = true;
        if (attackComponent is null)
            CheckListArray[2] = false;
        else
            CheckListArray[2] = true;
        /* Check to make sure all Components are active and if not Delete and Try again
         * 
         */
        foreach (bool check in CheckListArray) {
            if (!check) {
                throw new Exception("Something");
            }
        }
        healthComponent.SubscribeToOnDeath(Death);
        navigationComponent.SubscribeToOnDistanceUpdate(attackComponent.UpdateDistanceToTarget);
            
    }
    
    
    public void Death() {
        //this.gameObject.SetActive(false);
        EnemySpawner.instance.RemoveFromCurrentList(threatScore);
        Destroy(gameObject);
    }
}
