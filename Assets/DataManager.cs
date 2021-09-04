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

    float HPcalculation(int HeavyCount, int LightCount, int GameTime) //Calculates HP
        {
        float HeavyPM = (HeavyCount*180) / GameTime;
        float LightPM = (LightCount*180) / GameTime;
        float Health = LightPM+(2*HeavyPM);
        if (Health == 0) { Health = 1; }
        return Health;
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
        int GameTime = Int32.Parse(data[1]);
        string P1attackstring = data[2];
        int[] P1attacks = split(P1attackstring);
        string P2attackstring = data[3];
        int[] P2attacks = split(P2attackstring);
        //Hp is calculated based on damage recieved
        float P1HP = HPcalculation(P1attacks[0], P1attacks[1], GameTime)*2000;
        float P2HP = HPcalculation(P2attacks[0], P2attacks[1], GameTime)* 2000;
        LoadAttackData(4000, 2000, P1HP, 4000, 2000, P2HP);
        int[][] SaveFileData = new int[3][];

        SaveFileData[0] = new int[] {GameTime};
        SaveFileData[1] = P1attacks;
        SaveFileData[2] = P2attacks;
        return SaveFileData;

    }
    void LoadAttackData(int P1Heavy, int P1Light, float P1HP, int P2Heavy, int P2Light, float P2HP)
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
        int[][] MatchAttackCount = GameManager.MatchAttackCount;
        string P1string = "p1 " + (OfflineTotal[1][0]+MatchAttackCount[0][0])+ " " + (OfflineTotal[1][1] + MatchAttackCount[0][1]);
        string P2string = "p2 " + (OfflineTotal[2][0] + MatchAttackCount[1][0]) + " " + (OfflineTotal[2][1] + MatchAttackCount[1][1]);
        string[] lines = { "false", (OfflineTotal[0][0]+GameManager.timer).ToString(), P1string, P2string };
        File.WriteAllLines("SaveFile.txt", lines);
    }

    void Start()
    {
        string[] Savedata = new string[4];
        Savedata =ReadSave("SaveFile.txt");
        if (Savedata[0] == "true") { Online = true; }
        else if (Savedata[0] == "false")
        {
            Online = false;
            if (Savedata[1] != "000") { OfflineTotal = OfflineStart(Savedata); }
        }
        GameManager.enabled= true; //starts game after data is loaded
    }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.GameWon) 
            {
                if (Online == false) { offlinewin(OfflineTotal); }
                if (Online == true) { }
            { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
            }
        }
    } 

