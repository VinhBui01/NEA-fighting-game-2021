using System.IO;
using UnityEngine;
public class StartManager : MonoBehaviour
{
    //public int Gamemode;
    public void offline()
    {

        Clear("false");
    }

    public void Clear(string GameModeOnline) //reformats text file to empty
    {
        string[] lines = { GameModeOnline, "000", "P1 000 000", "P2 000 000" };
        File.WriteAllLines("SaveFile.txt", lines);
    }
    public void online()
    {
        Clear("true");
    }
}
