using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{

    [SerializeField] private float viewRadius;
    [Range(0, 360)]
    public float viewAngle; 
    public float range;
    [SerializeField] private float fovCheckTime = 0.2f;
    public float alertMaxTime = 5;
    public float alertCountdown = 5;
    public int difficulty;
    public float enemyReactionTime = 1;
    public float health = 40f;
    

    private GameObject player;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask wallsMask;
    [SerializeField] private EnemyGunScript enemyGun;
    [SerializeField] private Light alertLight;
    [SerializeField] private Rigidbody rb;
    public EnemyPatrol patrolBehavior;

    [SerializeField] private bool canSeePlayer;
    [SerializeField] private float enemySpeed = 5;
    [SerializeField] private bool alert;
    [SerializeField] private bool aggressive;
    [SerializeField] private bool currentlySearching;
    private Vector3 lastLocation =new Vector3 (0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (canSeePlayer)
        {
            alertLight.color = new Color(1, 1, 0);
            //transform.position = transform.position + ((player.transform.position - transform.position).normalized * Time.deltaTime) * enemySpeed;
            MoveTowardsPoint(player.transform.position);
            transform.LookAt(player.transform);
            alert = true;
            alertCountdown = alertMaxTime;

            StartCoroutine(ReactionTime());
        }

        if (alert && !canSeePlayer)
        {
            SearchForPlayer();
            enemyGun.enemyAttack = false;
        }


        if (!canSeePlayer
        {
            enemyGun.enemyAttack = false;
        }

        if(Input.GetMouseButtonDown(0) && (Vector3.Distance(transform.position, player.transform.position) < range ))
        {
            canSeePlayer = true;
            lastLocation = player.transform.position;
            MoveTowardsPoint(lastLocation);
            //Debug.Log((Vector3.Distance(transform.position, player.transform.position)));
        }


    }






    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfView();
        }
        
    }

    private IEnumerator AlertCooldown()
    {
        yield return alert = true;

        if (alert == true)
        {
            alertCountdown -= Time.deltaTime;
            //Debug.Log(alertCountdown);

            if (alertCountdown <= 0)
            {
                alert = false;
                //Debug.Log("Must've been the wind");
                lastLocation = new Vector3(0, 0, 0);
                alertLight.color = new Color(0, 1, 0);
                patrolBehavior.GotoNextPosition();
            }
        }
    }

    private IEnumerator ReactionTime()
    {
        float newReactionTime = enemyReactionTime - (difficulty / 10);
        yield return new WaitForSeconds(newReactionTime);
        enemyGun.enemyAttack = true;
        AttackPlayer();
    }

    //Checks to see if the player is within the range of the enemy
    //If they are, we check to see if the player is within line of sight
    //If they are, make a raycast to see if the player is directly in front of the enemy. Checks to see if the player is behind a wall
    private void FieldOfView()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        if (rangeChecks.Length != 0)
        {

            for (int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, wallsMask))
                    {
                        canSeePlayer = true;
                    }
                    else
                        canSeePlayer = false;
                }
                else
                    canSeePlayer = false;

            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void MoveTowardsPoint(Vector3 movingToPoint)
    {
        transform.position = transform.position + ((movingToPoint - transform.position).normalized * Time.deltaTime) * enemySpeed;
    }

    private void SearchForPlayer()
    {
        if (lastLocation == new Vector3(0f, 0f, 0f))
        {
            lastLocation = player.transform.position;
        }
        transform.LookAt(player.transform);
        currentlySearching = true;
        StartCoroutine(AlertCooldown());
        patrolBehavior.AggressiveBehavior();

    }

    private void AttackPlayer()
    {

        enemyGun.enemyAttack = true;
        alertLight.color = new Color(1, 0, 0);
        patrolBehavior.AggressiveBehavior();
    }

	//If this object is moving faster than intended (from some sort of knockback), take damage equal to force once they hit another object
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 100)
        {
            TakeDamage(collision.relativeVelocity.magnitude);
        }
    }

    public void TakeDamage(float damageTaken)
    {
        //Debug.Log("OUCH CHARLIE THAT HURTS");
        health -= damageTaken;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
