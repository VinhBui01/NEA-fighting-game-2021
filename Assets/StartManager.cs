using System.IO;
using UnityEngine;
using UnityEngine.Networking;
public class StartManager : MonoBehaviour
{
    public string[] P1Login;
    public string[] P2Login;
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
    public void online()
    {
        WWWForm form = new WWWForm();
        form.AddField("P1name", P1Login[0]);
        form.AddField("P1password", P1Login[1]);
        form.AddField("P2name", P2Login[0]);
        form.AddField("P2password", P2Login[1]);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == "000")
        {
            Debug.Log("Registration Successful");
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.downloadHandler.text);
        }
        www.Dispose();
    }
}
