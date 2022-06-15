using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    [SerializeField] private GameManager gameManager;
    public Slider healthSlider;
    public Slider deadHealthSlider;
    
    

    [SerializeField] private float health = 100f;
    [SerializeField] private bool runToggle = false;
    public float moveSpeed = 10f;
    public float walkSpeed = 10f;
    public float runSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 100f;
    public float maxHealth;
    [SerializeField] private float deadHealth;

    private bool canRegainHealth;
    private bool tired = false;
    [SerializeField] private float healthRegenTimer;
    [SerializeField] private float victorHealthRegen = 20;
    [SerializeField] private float ivyHealthRegen = 15;

    [SerializeField] private Transform kick;
    [SerializeField] private float kickRadius = 3f;
    [SerializeField] private float kickSpeed = 100f;
    private bool isKicking = false;


    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyLayer;
    private Vector3 velocity;
    private bool isGrounded;
    public PauseScript pauseScript;
    [SerializeField] private Animator anim;
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform playerTransform;



    //Code for leaning
    [SerializeField] private float leanDistance = 0;
    [SerializeField] private float maxLeanDistance = 20;
    [SerializeField] private int leanSpeed = 50;
    [SerializeField] private bool isLeaning = false;

    private void Start()
    {
        gameManager = GameManager.FindObjectOfType<GameManager>();

        if (gameManager.isCharacterIvy == true)
            IvySettings();
        else if (gameManager.isCharacterIvy == false)
            VictorSettings();

        healthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
 
        MovementSystem();
        HealthSystem();

        //Press jump to jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        //Running
        if ((Input.GetKey(KeyCode.LeftShift) && !tired) || runToggle && !tired)
        {
            moveSpeed = runSpeed;

            if (gameManager.isCharacterIvy == false)
            {
                health = health - Time.deltaTime * moveSpeed;

                if (health < 5)
                {
                    tired = true;
                    StartCoroutine(CatchingBreath(healthRegenTimer));
                }
            }

            if(runToggle && Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = walkSpeed;
            }
        }
        else
            moveSpeed = walkSpeed;

        if(Input.GetKeyUp(KeyCode.LeftShift) && !gameManager.isCharacterIvy)
        {
            tired = true;
            StartCoroutine(CatchingBreath(healthRegenTimer / 4));
        }

        if(Input.GetKeyDown(KeyCode.F) && gameManager.isCharacterIvy == true && !isKicking)
        {
            StartCoroutine(BigKick());
            anim.SetBool("Kick", true);
        }

        //Code for leaning
        if (isLeaning == false)
        {
            if(leanDistance > 0 && leanDistance != 0)
                leanDistance -= Time.deltaTime * (leanSpeed);
            else if(leanDistance <= 0 && leanDistance != 0)
                leanDistance += Time.deltaTime * (leanSpeed);

            if(leanDistance < 1 && leanDistance > 0)
                leanDistance = 0;
            else if (leanDistance > -1 && leanDistance < 0)
                leanDistance = 0;
        }
        

        if (Input.GetKey(KeyCode.Q) && gameManager.isCharacterIvy == false)
            Leaning(true);
        else if (Input.GetKey(KeyCode.E) && gameManager.isCharacterIvy == false)
            Leaning(false);
        else
            isLeaning = false;

        playerTransform.transform.RotateAround(playerTransform.position, playerTransform.forward, leanDistance);

    }





    public void MovementSystem()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = jumpHeight;
    }
    


    private IEnumerator BigKick()
    {
        isKicking = true;
        yield return new WaitForSeconds(0.25f);
        Collider[] peopleWeHit = Physics.OverlapSphere(kick.transform.position, kickRadius, enemyLayer);

        foreach(Collider enemyHit in peopleWeHit)
        {
            
            Vector3 startingPosition = enemyHit.transform.position;
            Rigidbody enemyHitRb = enemyHit.attachedRigidbody;
            Enemy enemyHitGameObj = enemyHit.GetComponent<Enemy>();

            enemyHitRb.AddForce(transform.forward * kickSpeed, ForceMode.Impulse);

        }
        isKicking = false;
        anim.SetBool("Kick", false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(kick.transform.position, kickRadius);   
    }



    public void HealthSystem()
    {
        healthSlider.value = health;
        
        if(gameManager.isCharacterIvy && health < maxHealth)
        {
            health = health + (ivyHealthRegen * Time.deltaTime);
        }

        if(!gameManager.isCharacterIvy)
        {
            if(!tired && canRegainHealth && (health < maxHealth) && moveSpeed < runSpeed)
            {
                health += victorHealthRegen * Time.deltaTime;
                
            }
            deadHealthSlider.value = deadHealth;
        }

        if (!canRegainHealth)
        {
            StartCoroutine(WaitForHealthRegen(healthRegenTimer));
        }
    }



    private void Leaning(bool leanLeft)
    {


        isLeaning = true;
        //playerTransform.RotateAround(transform.position, Vector3.forward, leanDistance);

        if (isLeaning && !leanLeft && leanDistance > -maxLeanDistance)
        {
            leanDistance -= Time.deltaTime * (leanSpeed);
            //playerTransform.transform.RotateAround(playerTransform.position, playerTransform.right, leanDistance);
            //playerTransform.Rotate(Vector3.forward, leanDistance);
        }
        else if (isLeaning && leanLeft && leanDistance < maxLeanDistance)
        {
            leanDistance += Time.deltaTime * (leanSpeed);
            //playerTransform.RotateAround(playerTransform.position, playerTransform.right, leanDistance);
            //playerTransform.Rotate(Vector3.forward, leanDistance);
        }
    }


        public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        
        //Debug.Log("We're taking damage");
        if (health <= 0)
        {
            Die();
        }

        if (!gameManager.isCharacterIvy)
        {
            deadHealth += damageTaken;
            maxHealth -= damageTaken;
        }
    }

    void Die()
    {
        pauseScript.GameOver();
        //Destroy(gameObject);
    }

    IEnumerator WaitForHealthRegen(float waitBeforeHealthRegens)
    {
        yield return new WaitForSeconds(waitBeforeHealthRegens);
        canRegainHealth = true;
    }

    IEnumerator CatchingBreath(float tiredTimer)
    {
        yield return new WaitForSeconds(tiredTimer);
        tired = false;
    }

    public void VictorSettings()
    {
        maxHealth = 100;
        health = maxHealth;
        moveSpeed = 5f;
        walkSpeed = 5f;
        runSpeed = 15f;
        gravity = -25f;
        jumpHeight = 10f;
        healthRegenTimer = 2.5f;
    }
    public void IvySettings()
    {
        moveSpeed = 20f;
        walkSpeed = 20f;
        runSpeed = 40f;
        gravity = -22;
        jumpHeight = 12f;
        maxHealth = 200;
        health = maxHealth;
        healthRegenTimer = 0.25f;
    }

    public void AutoSprint()
    {
        if(runToggle)
        {
            runToggle = false;
        }
        else
            runToggle = true;

    }

}
