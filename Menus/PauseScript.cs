using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenu;
    public PlayerController playerControl;
    private GameManager gameManager;
    public bool gameOver;
    public TMP_Text gameOverText;

    public void Start()
    {
        gameManager = GameManager.FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
        pauseMenu.SetActive(false);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
        pauseMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
	
	//Character selection. Resets the scene once the character is changed
    public void ChooseIvy()
    {
        gameManager.isCharacterIvy = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        playerControl.IvySettings();
        Resume();

    }
    public void ChooseVictor()
    {
        gameManager.isCharacterIvy = false; ;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        playerControl.VictorSettings();
        Resume();
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        Pause();
    }
}
