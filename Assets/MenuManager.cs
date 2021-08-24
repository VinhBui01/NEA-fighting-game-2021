using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); } //moves to next scene

    public void QuitGame()
    {
        Debug.Log("Application Terminated");
        Application.Quit();
    }
    void Update()
    {
        
    }
}
