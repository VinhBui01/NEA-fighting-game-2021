using System.IO;
using UnityEngine;
public class StartManager : MonoBehaviour
{
    //public int Gamemode;
    public bool GameModeOnline;
    public void offline()
    {

        GameModeOnline =  false;
        Clear();
    }

    public void Clear() //reformats text file to empty
    {
        string[] lines = { "000", "P1 000 000", "P2 000 000" };
        File.WriteAllLines("SaveFile.txt", lines);
    }
    public void online()
    {
        GameModeOnline = true;
    }

}
