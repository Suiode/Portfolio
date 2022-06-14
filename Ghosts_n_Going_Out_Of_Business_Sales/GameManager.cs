using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxFrameRate = 60;
    public static GameManager instance;


    //Slow down variables for ghosts
    public bool slowDownActive;
    [SerializeField] private float slowDownLength = 2f;
    [SerializeField] private float slowDownTimer = 2f;



    //public PauseScript pauseScript;

    // Start is called before the first frame update
    void Start()
    {
        //pauseScript = GameObject.FindObjectOfType<PauseScript>();

        //Makes sure the GameManager is kept between scenes. If there isn't one, then create it
        DontDestroyOnLoad(gameObject);

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = maxFrameRate;
    }
    
    public void StartSlowDown()
    {
        if (!slowDownActive)
        {
            StartCoroutine(SlowDownEnemies());
        }
        else
            slowDownTimer = slowDownLength;


    }

    public IEnumerator SlowDownEnemies()
    {
        Debug.Log("We have started the slow down script");

        slowDownTimer = slowDownLength;
        slowDownActive = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Tell enemies to slow down
        for (int i = 0; i < enemies.Length; i++)
        {
            GhostFace enemyScript = enemies[i].GetComponent<GhostFace>();
            enemyScript.SlowDownTime();
            Debug.Log("Slowing down for ghosties");
        }

        while (slowDownActive)
        {
            //Debug.Log("Made it to the counter");
            yield return null;
            slowDownTimer -= Time.deltaTime;


            //Debug.Log("WaitForSeconds completed successfully");
            if (slowDownTimer <= 0)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    GhostFace enemyScript = enemies[i].GetComponent<GhostFace>();
                    enemyScript.BackToNormalTime();
                    Debug.Log("Back to normal time");
                    slowDownActive = false;
                    slowDownTimer = 0;
                }
            }
        }
    }
}

