using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
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
        string[] lines = {"false", "000", "P1 000 000", "P2 000 000" };
        File.WriteAllLines("SaveFile.txt", lines);
    }
    IEnumerator online()
    {
        WWWForm form = new WWWForm();
        form.AddField("P1ID", P1ID);
        form.AddField("P2ID", P2ID);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/MatchupLogin.php", form);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == "200")
        {
            Debug.Log("Offline Start data recieved");
            string[] lines = { "true", "000", "P1 000 000", "P2 000 000" };
            File.WriteAllLines("SaveFile.txt", lines);
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.downloadHandler.text);
        }
        www.Dispose();
    }
}
