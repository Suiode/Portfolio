using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public GameObject interactPromptTransform;
    public TextMeshPro interactPromptText;
    public GameObject playerHand;
    [SerializeField] private PlayerController player;
    [SerializeField] private bool canPlayerInteract;

    public LayerMask playerMask;
    public LayerMask wallsMask;


    private void Start()
    {
        //interactCollider.radius = radius;
        //sphereCollider.radius = radius;
        player = PlayerController.FindObjectOfType<PlayerController>();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && interactPromptTransform.active && canPlayerInteract)
            InteractExecute();

        CheckObjectsInRange();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }




    private void CheckObjectsInRange()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerMask);

        if (rangeChecks.Length != 0)
        {
            Transform playerTransform = rangeChecks[0].transform;
            EnableInteract(playerTransform, interactPromptTransform);
            interactPromptTransform.transform.rotation = Quaternion.LookRotation(interactPromptTransform.transform.position - playerTransform.transform.position);

        }
        else
            DisableInteract();
    }


    private void EnableInteract(Transform player, GameObject prompt)
    {
        prompt.SetActive(true);
    }


    private void DisableInteract()
    {
        interactPromptTransform.SetActive(false);
    }
    

    private void InteractExecute()
    {
        if (!player.godMode)
        {
            player.GodMode();
            playerHand.SetActive(false);
        }
        else
        {
            player.GodMode();
            playerHand.SetActive(true);
        }

    }
}
