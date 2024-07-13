using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DebugLevelButton()
    {
        SceneManager.LoadScene("SampleScene");
        print("DebugLevel Button Pressed");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("MainScene");
        print("StartLevel Button Pressed");
    }
}
