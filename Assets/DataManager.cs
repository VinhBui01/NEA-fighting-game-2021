using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class DataManager : MonoBehaviour
{
    string MatchupID;
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

        float Health = (LightCount+2*HeavyCount)*180/GameTime;
        if (Health == 0) { return 4000; }
        else { return Health*2000; }
        }
    int[] split(string playerstring)  //seperates the string into an integer array
    {
        string[] datasplit = playerstring.Split(' ');
        int[] playerint = new int[2];
        playerint[0] = Int32.Parse(datasplit[1]);
        playerint[1] = Int32.Parse(datasplit[2]);
        return playerint;
    }

    int[][] DataParse(string[] data) // loads data values to game manager
    {
        int GameTime = Int32.Parse(data[1]);
        string P1attackstring = data[2];
        int[] P1attacks = split(P1attackstring);
        string P2attackstring = data[3];
        int[] P2attacks = split(P2attackstring);
        //Hp is calculated based on damage recieved
        if (data[1] != "000")
        //repeated runs
        {
            float P1HP = HPcalculation(P1attacks[0], P1attacks[1], GameTime);

            float P2HP = HPcalculation(P2attacks[0], P2attacks[1], GameTime);

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
        GameManager.P2HP = P2HP;
        GameManager.P2Heavy = P2Heavy;
        GameManager.P2Light = P2Light;
    }
    void offlinewin (int[][] OfflineTotal)
    {
        string P1string = "p1 " + (OfflineTotal[1][0] + GameManager.P1HeavyCount) + " " + (OfflineTotal[1][1] + GameManager.P1LightCount);
        string P2string = "p2 " + (OfflineTotal[2][0] + GameManager.P2HeavyCount) + " " + (OfflineTotal[2][1] + GameManager.P2LightCount);
        string[] lines = { "false", (OfflineTotal[0][0] + GameManager.timer).ToString(), P1string, P2string };
        File.WriteAllLines("SaveFile.txt", lines);
    }
 IEnumerator onlineend()//loads data to database
    {
        WWWForm form = new WWWForm();
        form.AddField("MatchupID", MatchupID);
        form.AddField("P1H", GameManager.P1HeavyCount);
        form.AddField("P1L", GameManager.P1LightCount);
        form.AddField("P2H", GameManager.P2HeavyCount);
        form.AddField("P2L", GameManager.P2LightCount);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/AddMatches.php", form);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == "300")
        {
            Debug.Log("Registration Successful");
        }
        else
        {
            Debug.Log("Match addition failed. Error #" + www.downloadHandler.text);
        }
        www.Dispose();
    }

    void Start()
    {
        string[] Savedata = new string[4]; //reads save file data
        Savedata = ReadSave("SaveFile.txt");
        if (Savedata[0] == "true") { Online = true; }
        else if (Savedata[0] == "false")
        {
            Online = false;
        }
        MatchupID = Savedata[4];
        OfflineTotal = DataParse(Savedata);
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
                string[] lines = { "false", (OfflineTotal[0][0] + (GameManager.timer)/60).ToString(), P1string, P2string };
                File.WriteAllLines("SaveFile.txt", lines);
            }

            else if (Online == true) 
            { 
            
            }
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
            
        }
    } 

