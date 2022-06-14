using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int destPoint = 0;
    public NavMeshAgent navAgent;
    public float minimumDistanceToPoint = 0.5f;
    public Enemy enemy;
    //public PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        //Auto-breaking can be disabled if we don't want the enemy stopping between points
        navAgent.autoBraking = true;
        GotoNextPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(!navAgent.pathPending && navAgent.remainingDistance < minimumDistanceToPoint)
        {
            GotoNextPosition();
        }
    }



    public void GotoNextPosition()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }
        navAgent.destination = patrolPoints[destPoint].position;
        destPoint = (destPoint + 1) % patrolPoints.Length;
    }

    public void AggressiveBehavior()
    {
        navAgent.destination = enemy.lastLocation;
        //Debug.Log("Last player location: " + enemy.lastLocation);
    }
}
