using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame(int SceneNumber) //moves to next scene
    { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + SceneNumber); } 

    public void QuitGame() //quits game
    {
        Debug.Log("Application Terminated");
        Application.Quit();
    }

}
