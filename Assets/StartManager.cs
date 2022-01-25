using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public Registration Registration;
    public string P1ID = "-1";
    public string P2ID = "-1";
    public string RecordID;
    //public int Gamemode;
    public void offline()
    {

        Clear();
    }

    public void Clear() //reformats text file to empty for offline start
    {
        string[] lines = {"false", "000", "P1 000 000", "P2 000 000" , "null"};
        File.WriteAllLines("SaveFile.txt", lines);
    }


    public void onlinestart()
    {
        if (P1ID == "-1" | P2ID == "-1" | P2ID == P1ID)
        {
            Debug.Log("Invaild Users");
        }
        else
        {
            StartCoroutine(findID());
        }
    }
    IEnumerator findID()//loads data from database to save file
    {
        WWWForm form = new WWWForm();
        form.AddField("P1ID", P1ID);
        form.AddField("P2ID", P2ID);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/locatematchupID.php", form);
        yield return www.SendWebRequest();
        if ((www.downloadHandler.text).Split('\t')[0] == "200")
        {
            RecordID = (www.downloadHandler.text).Split('\t')[1];
            www.Dispose();
            StartCoroutine(loadmatchup(1));
        }
        else
        {
            Debug.Log("MatchupID search failed. Error #" + www.downloadHandler.text);
            www.Dispose();
        }
    }

    public IEnumerator loadmatchup(int scene)//loads data from database to save file
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", RecordID);
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
            string[] lines = { "true", data[0], ("P1 " + data[1] + " " + data[2]), ("P2 " + data[3] + " " + data[4]), RecordID };

            File.WriteAllLines("SaveFile.txt", lines);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + scene);
        }
        else
        {
            Debug.Log("Match start failed. Error #" + www.downloadHandler.text);

        }
        www.Dispose();
    }
}
