using System.Collections;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using MenuManager;
public class DataManager : MonoBehaviour
{
    string MatchupID;
    int[][] OfflineTotal = new int[3][];
    bool Online;
    public GameManager GameManager;
    public TMP_Text P1HPUI;
    public TMP_Text P2HPUI;
    public TMP_Text P1LUI;
    public TMP_Text P2LUI;
    public TMP_Text P1HUI;
    public TMP_Text P2HUI;

    void pregame()
    {
        P1HPUI.text = "HP: " + GameManager.P1HP;
        P2HPUI.text = "HP: " + GameManager.P2HP;
        P1LUI.text = "LD: " + GameManager.P1Light;
        P2LUI.text = "LD: " + GameManager.P2Light;
        P1HUI.text = "HD: " + GameManager.P1Heavy;
        P2HUI.text = "HD: " + GameManager.P2Heavy;
    }
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
        float Health = (LightCount + 2 * HeavyCount) * 180 / GameTime;
        if (Health == 0) { return 4000; }
        else { return Health * 2000; }
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
        if (Int32.Parse(data[1]) != 0)
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
        SaveFileData[0] = new int[] { GameTime };
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
    void offlinewin(int[][] OfflineTotal)
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
            form.AddField("Time", GameManager.timer/60);
            form.AddField("P1H", GameManager.P1HeavyCount);
            form.AddField("P1L", GameManager.P1LightCount);
            form.AddField("P2H", GameManager.P2HeavyCount);
            form.AddField("P2L", GameManager.P2LightCount);


        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/AddMatches.php", form);
            yield return www.SendWebRequest();
            if (www.downloadHandler.text == "300")
            {
                Debug.Log("Upload Match Data successfull");
            StartCoroutine(loadmatchup(1));
            }
            else
            {
                Debug.Log("Match addition failed. Error #" + www.downloadHandler.text);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            
        }
            www.Dispose();
        }
        public IEnumerator loadmatchup(int scene)//loads data from database to save file
        {
            WWWForm form = new WWWForm();
            form.AddField("ID", MatchupID);
            UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/findmatchdata.php", form);
            yield return www.SendWebRequest();
            if ((www.downloadHandler.text).Split('\t')[0] == "300")
            {
                string[] data = new string[5];
                for (int i = 1; i < 6; i++)
                {
                    data[i - 1] = (www.downloadHandler.text).Split('\t')[i];
                }
                Debug.Log("online Start data recieved");
                string[] lines = { "true", data[0], ("P1 " + data[1] + " " + data[2]), ("P2 " + data[3] + " " + data[4]), MatchupID };

                File.WriteAllLines("SaveFile.txt", lines);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + scene);
            }
            else
            {
                Debug.Log("Match start failed. Error #" + www.downloadHandler.text);

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
            pregame();
        }

        // Update is called once per frame


        void Update()
        {
            if (GameManager.GameWon)
            {
            GameManager.enabled = false;
            GameManager.GameWon = false;
                if (Online == false)
                {
                    string P1string = "p1 " + (OfflineTotal[1][0] + GameManager.P1HeavyCount).ToString() + " " + (OfflineTotal[1][1] + GameManager.P1LightCount).ToString();
                    string P2string = "p2 " + (OfflineTotal[2][0] + GameManager.P2HeavyCount).ToString() + " " + (OfflineTotal[2][1] + GameManager.P2LightCount).ToString();
                    string[] lines = { "false", (OfflineTotal[0][0] + (GameManager.timer) / 60).ToString(), P1string, P2string, "null"};
                    File.WriteAllLines("SaveFile.txt", lines);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else if (Online == true)
                {
                    StartCoroutine(onlineend());
                }


            }
        }
    }

