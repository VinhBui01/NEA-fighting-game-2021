using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    public List<string> ReadSave(string filename)
    {
        List<string> Text = new List<string>();
        try
        {
            string[] readText = File.ReadAllLines(filename);
            foreach (string s in readText)
            {
                Text.Add(s);
            }
        }
        catch (FileNotFoundException e)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        return Text;
    }
    void Start()
        {
        ReadSave("SaveFile.txt");
        }

        // Update is called once per frame
        void Update()
        {

        }
    } 

