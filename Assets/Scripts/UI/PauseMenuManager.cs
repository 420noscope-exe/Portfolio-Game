using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private bool isPaused;
    private GameObject pauseMenu;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu = GameObject.Find("PauseMenu");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CursorLockInput();
        SetLockState();
    }

    void FixedUpdate() //Don't put any code in here, it won't run when the game is paused.
    {
        //SetLockState();
    }

    public void CursorLockInput()
    {
    	if(Input.GetButtonDown("Menu"))
    	{
            print("Menu Button Pressed");
    		isPaused = !isPaused;
    	}
    }

    public void SetLockState()
    {
        if(isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Pause();
        }
        else if(!isPaused && !player.GetComponent<PlayerHealthController>().IsDead())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Resume();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        isPaused = false;
        print("Resume Button Pressed");
    }
    
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Restart Button Pressed");
    }
}
