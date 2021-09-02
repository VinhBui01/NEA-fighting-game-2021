using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    public string[] ReadSave(string filename)
    {
        try
        {
            string[] ReadText = File.ReadAllLines(filename);
        return ReadText;
        }
        catch (FileNotFoundException)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            return null;
        }
    }
    void Start()
        {
        string[] data = new string[4];
        data =ReadSave("SaveFile.txt");
        if (data[0] == "true") { }
        else if (data[0] == "false")
        {
            if (data[1] == "000") { }
            else { }
        }

    }

        // Update is called once per frame
        void Update()
        {

        }
    } 

