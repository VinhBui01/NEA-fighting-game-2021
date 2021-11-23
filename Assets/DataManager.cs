using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    int[][] OfflineTotal = new int[3][];
    bool Online;
    public GameManager GameManager;
    public string[] ReadSave(string filename) //tries to open file
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

    float HPcalculation(int HeavyCount, int LightCount, int GameTime) //Calculates HP
        {
        Debug.Log("L"+LightCount); Debug.Log("H"+HeavyCount);
        float HeavyPM = (HeavyCount*180*4000*60)/GameTime;
        float LightPM = (LightCount*180*2000*60)/GameTime;
        float Health = LightPM+(HeavyPM);
        if (Health == 0) { return 2000; }
        else { return Health; }
        }
    int[] split(string playerstring)  //seperates the string into an integer array
    {
        string[] datasplit = playerstring.Split(' ');
        int[] playerint = new int[2];
        playerint[0] = Int32.Parse(datasplit[1]);
        playerint[1] = Int32.Parse(datasplit[2]);
        return playerint;
    }

    int[][] OfflineStart(string[] data)
    {
        Debug.Log(data[0]); Debug.Log(data[1]); Debug.Log(data[2]); Debug.Log(data[3]);
        int GameTime = Int32.Parse(data[1]);
        string P1attackstring = data[2];
        int[] P1attacks = split(P1attackstring);
        string P2attackstring = data[3];
        int[] P2attacks = split(P2attackstring);
        //Hp is calculated based on damage recieved
        if (data[1] != "000")
        //repeated runs
        {
            Debug.Log(GameTime);
            float P1HP = HPcalculation(P1attacks[0], P1attacks[1], GameTime);
            float P2HP = HPcalculation(P2attacks[0], P2attacks[1], GameTime);
            Debug.Log("H1"+ P1HP); Debug.Log("H2" + P2HP);
            LoadAttackData(4000, 2000, P1HP, 4000, 2000, P2HP);
        }
        else
        //initial run
        {
            float P1HP = 32000; 
            float P2HP = 32000;
            LoadAttackData(4000, 2000, P1HP, 4000, 2000, P2HP);
        }
        int[][] SaveFileData = new int[3][];
        //returns old save file data
        SaveFileData[0] = new int[] {GameTime};
        SaveFileData[1] = P1attacks;
        SaveFileData[2] = P2attacks;
        return SaveFileData;

    }
    void LoadAttackData(int P1Heavy, int P1Light, float P1HP, int P2Heavy, int P2Light, float P2HP)//loads data to  game manager
    {
        GameManager.P1HP = P1HP;
        GameManager.P1Heavy = P1Heavy;
        GameManager.P1Light = P1Light;
        GameManager.P2HP = P1HP;
        GameManager.P2Heavy = P1Heavy;
        GameManager.P2Light = P1Light;
    }
    void offlinewin (int[][] OfflineTotal)
    {
        string P1string = "p1 " + (OfflineTotal[1][0] + GameManager.P1HeavyCount) + " " + (OfflineTotal[1][1] + GameManager.P1LightCount);
        string P2string = "p2 " + (OfflineTotal[2][0] + GameManager.P2HeavyCount) + " " + (OfflineTotal[2][1] + GameManager.P2LightCount);
        string[] lines = { "false", (OfflineTotal[0][0] + GameManager.timer).ToString(), P1string, P2string };
        File.WriteAllLines("SaveFile.txt", lines);
    }

    void Start()
    {
        string[] Savedata = new string[4]; //reads save file data
        Savedata = ReadSave("SaveFile.txt");
        if (Savedata[0] == "true") { Online = true; }
        else if (Savedata[0] == "false")
        {
            Online = false;
            { OfflineTotal = OfflineStart(Savedata); }

        }
        GameManager.enabled= true; //starts game after data is loaded
    }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.GameWon) 
            {
                if (Online == false) 
            {
                string P1string = "p1 " + (OfflineTotal[1][0] + GameManager.P1HeavyCount).ToString() + " " + (OfflineTotal[1][1] + GameManager.P1LightCount).ToString();
                string P2string = "p2 " + (OfflineTotal[2][0] + GameManager.P2HeavyCount).ToString() + " " + (OfflineTotal[2][1] + GameManager.P2LightCount).ToString();
                string[] lines = { "false", (OfflineTotal[0][0] + GameManager.timer).ToString(), P1string, P2string };
                File.WriteAllLines("SaveFile.txt", lines); ;
            }
                if (Online == true) { }
            { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
            }
        }
    } 

