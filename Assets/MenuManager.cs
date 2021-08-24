using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame() //moves to next scene
    { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); } 

    public void QuitGame() //quits game
    {
        Debug.Log("Application Terminated");
        Application.Quit();
    }

}
