using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using MenuManager;
using UnityEngine.SceneManagement;
public class StartManager : MonoBehaviour
{
    public string P1ID;
    public string P2ID;
    //public int Gamemode;
    public void offline()
    {

        Clear();
    }

    public void Clear() //reformats text file to empty for offline start
    {
        string[] lines = {"false", "000", "P1 000 000", "P2 000 000" , ""};
        File.WriteAllLines("SaveFile.txt", lines);
    }


    public void onlinestart()
    {
        if (P1ID == null | P2ID == null | P2ID != P1ID)
        {
            Debug.Log("Invaild Users");
        }
        else
        {
            StartCoroutine(online());
        }
    }
    IEnumerator online()//loads data from database to save file
    {
        WWWForm form = new WWWForm();
        form.AddField("P1ID", P1ID);
        form.AddField("P2ID", P2ID);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/MatchupLogin.php", form);
        yield return www.SendWebRequest();
        if ((www.downloadHandler.text).Split('\t')[0] == "200")
        {
            string[] data = new string[6];
            for (int i = 1; i < 7; i++)
            {
                data[i-1] = (www.downloadHandler.text).Split('\t')[i];
            }
            Debug.Log("Offline Start data recieved");
            string[] lines = { "true", data[1], ("P1 "+ data[2] + " "+  data[3]), ("P1 "+ data[4] + " " + data[5]), data[6]};
            File.WriteAllLines("SaveFile.txt", lines);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.downloadHandler.text);
        }
        www.Dispose();
    }
}
