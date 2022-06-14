using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    //public Slider healthSlider;


    public float health = 100f;
    public float moveSpeed = 5f;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float currentGravity = -20f;
    [SerializeField] float defaultGravity = -20f;
    public float jumpHeight = 10f;
    public float maxHealth;

    private bool canRegainHealth;
    public float healthRegenTimer;
    public float healthRegenSpeed = 15;


    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    public LayerMask enemyLayer;
    Vector3 velocity;
    public bool isGrounded;
    public Animator anim;
    public Camera playerCam;
    public Transform playerTransform;

    public bool godMode = false;



    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (!godMode)
        {
            MovementSystem();
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            //HealthSystem();
        }
        else
        {
            GodModeMovement();
        }


        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();


        //Running
        if ((Input.GetKey(KeyCode.LeftShift)))
        {
            moveSpeed = runSpeed;
        }
        else
            moveSpeed = walkSpeed;

        

    }





    public void MovementSystem()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void Jump()
    {
        velocity.y = jumpHeight;
    }



    /*public void HealthSystem()
    {
        //healthSlider.value = health;

        if (health < maxHealth)
            health = health + (healthRegenSpeed * Time.deltaTime);

        if (!canRegainHealth)
            StartCoroutine(WaitForHealthRegen(healthRegenTimer));

    }*/


    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        //Currently commented out, but will be enabled when pausing is enabled
        //pauseScript.GameOver();
    }

    /*IEnumerator WaitForHealthRegen(float waitBeforeHealthRegens)
    {
        yield return new WaitForSeconds(waitBeforeHealthRegens);
        canRegainHealth = true;
    }*/





    public void GodMode()
    {

        if (!godMode)
        {
            godMode = true;
            currentGravity = 0;
    
        }
        else
        {
            currentGravity = defaultGravity;
            godMode = false;
            Debug.Log("GodMode is disabled. Gravity is now set to " + currentGravity);
        }

    }


    private void GodModeMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            velocity.y += (moveSpeed * Time.deltaTime) * 2;

        if (Input.GetKey(KeyCode.Q))
            velocity.y -= (moveSpeed * Time.deltaTime) * 2;


        controller.Move(velocity * Time.deltaTime);


        

        //Make sure moving up and down don't happen forever
        if((velocity.y > 0 || velocity.y < 0) && !(Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)))
        {
            velocity.y = 0;
        }    
 
    }



}
