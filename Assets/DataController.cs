using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class DataController:MonoBehaviour
{
    //public int Gamemode;
    public bool GameModeOnline;
    public void offline()
    {
        GameModeOnline=  false;
    }

    public void Clear() //reformats text file to empty
    {
        string[] lines = { "P1 0 0 0", "P2 0 0 0" };
        File.WriteAllLines("SaveFile.txt", lines);
        Debug.Log("done");
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
