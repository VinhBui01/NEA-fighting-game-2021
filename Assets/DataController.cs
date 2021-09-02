using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : GameManager
{
    //public int Gamemode;
    public bool GameModeOnline;
    public void offline()
    {
        GameModeOnline=  false;
    }

    public void Clear()
    {
        string[] lines =
        {
            "P1 0 0 0", "P2 0 0 0"
        };

        File.WriteAllLines(@"C:unityprojects\NEA fighting game 2021\SaveFile.txt", lines);
    }
    public void online()
    {
        GameModeOnline = true;
    }
    private void Start()
    {
        Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
