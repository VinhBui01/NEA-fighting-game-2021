using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    public List<string> ReadSave()
    {
        List<string> Text = new List<string>();
        try
        {
            string[] readText = File.ReadAllLines("Sile.txt");
            foreach (string s in readText)
            {
                Text.Add(s);
            }
        }

        catch (FileNotFoundException e)
        {
            Debug.Log("penis");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        return Text;
    }
    void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    } 

