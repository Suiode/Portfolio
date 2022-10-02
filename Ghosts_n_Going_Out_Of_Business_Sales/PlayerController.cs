using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    public float health = 100f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashMaxTime = 0.5f;
    [SerializeField] private bool dashing = false;


    [Header("Jumping and gravity")]
    public float currentGravity = -20f;
    [SerializeField] private float defaultGravity = -20f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float jumpSpeed = 0.25f;
    [SerializeField] private float jumpMaxTimer = 1f;
    [SerializeField] private float groundDistance = 0.3f;
    private Vector3 velocity;
    [SerializeField] private bool isGrounded;


    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private bool canRegainHealth;
    [SerializeField] private float healthRegenTimer;
    [SerializeField] private float healthRegenSpeed = 15;


    [Header("Game Objects")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Animator anim;
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController controller;

    [Header("Misc")]
    [SerializeField] private bool godMode = false;



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
        {
            StartCoroutine(Jump());
        }


        //Dash
        if ((Input.GetKeyDown(KeyCode.LeftShift) && !dashing))
        {
            StartCoroutine(Dash(moveSpeed));
        }


    }





    public void MovementSystem()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //if (velocity.y > currentGravity)
        //{
        //    velocity.y += currentGravity;
        //}
        //else
        //{
        velocity.y += currentGravity;

        if (velocity.y < currentGravity)
        {
            velocity.y = currentGravity;
        }
        //}

        Vector3 move = (transform.right * x) * moveSpeed + (transform.forward * z) * moveSpeed + (velocity);

        controller.Move(move * Time.deltaTime);

        //controller.Move(velocity * Time.deltaTime);
    }

    private IEnumerator Jump()
    {
        float timer = 0;
        moveSpeed *= 2f;
        //velocity.y += jumpHeight;


        //while(velocity.y < jumpHeight)
        //{
        //    yield return null;
        //    //velocity.y += jumpHeight * moveSpeed;
        //    velocity.y = Mathf.Lerp(velocity.y, jumpHeight, jumpSpeed);
        //}

        while (timer <= jumpMaxTimer)
        {
            yield return null;
            timer += Time.deltaTime;

            velocity.y = Mathf.Lerp(velocity.y, Mathf.Abs(currentGravity) * jumpHeight, jumpSpeed - timer);

            if (isGrounded && timer > 0.5f)
            {
                break;
            }
        }


        moveSpeed = walkSpeed;
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
        if ((velocity.y > 0 || velocity.y < 0) && !(Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)))
        {
            velocity.y = 0;
        }

    }


    private IEnumerator Dash(float curMoveSpeed)
    {
        moveSpeed = curMoveSpeed * dashSpeed;
        float dashTimer = 0;
        dashing = true;

        yield return null;


        while (dashing)
        {
            dashTimer += Time.deltaTime;


            if (dashTimer > dashMaxTime)
            {
                dashing = false;
                moveSpeed = curMoveSpeed;
            }

        }

    }

}
