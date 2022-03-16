using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public string P1ID = "-1"; //-1 is used as a null value as all player IDS are positive
    public string P2ID = "-1"; //-1 is used as a null value as all player IDS are positive
    public string RecordID; //where the record ID will be stored
    public void offline()
    {

        Clear();
    }

    public void Clear() //reformats text file to empty for offline start
    {
        string[] lines = {"false", "000", "P1 000 000", "P2 000 000" , "null"};
        File.WriteAllLines("SaveFile.txt", lines);
    }


    public void onlinestart() //this is attatched to the online start button
    {
        if (P1ID == "-1" | P2ID == "-1" | P2ID == P1ID) //all these conditions need to be false to start online
        {
            Debug.Log("Invaild Users"); //outputs error message
        }
        else
        {
            StartCoroutine(findID());
        }
    }
    IEnumerator findID() //loads data from database to save file
    {
        WWWForm form = new WWWForm(); //create new form
        form.AddField("P1ID", P1ID); //attach P1 ID to form
        form.AddField("P2ID", P2ID); //attatch P2 ID to form
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/locatematchupID.php", form); //starts unity connection
        yield return www.SendWebRequest(); //waits for response
        if ((www.downloadHandler.text).Split('\t')[0] == "200") //200 is success code
        {
            RecordID = (www.downloadHandler.text).Split('\t')[1]; //attatches record ID to variable
            www.Dispose(); // closes connection
            StartCoroutine(loadmatchup(1)); //starts coroutine to find match data
        }
        else
        {
            Debug.Log("MatchupID search failed. Error #" + www.downloadHandler.text);
            www.Dispose();
        }
    }

    public IEnumerator loadmatchup(int scene)//loads data from database to save file
    {
        WWWForm form = new WWWForm(); //creates new form
        form.AddField("ID", RecordID); //adds match ID
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/findmatchdata.php", form); //starts connection
        yield return www.SendWebRequest(); //waits for response
        if ((www.downloadHandler.text).Split('\t')[0] == "300") //300 is success code
        {
            string[] data = new string[5]; //creates an array
            for (int i = 1; i < 6; i++) // loops for the last 5 expected items of data from the code
            {
                data[i - 1] = (www.downloadHandler.text).Split('\t')[i]; // saves it to array
            }
            Debug.Log("online Start data recieved"); 
            string[] lines = { "true", data[0], ("P1 " + data[1] + " " + data[2]), ("P2 " + data[3] + " " + data[4]), RecordID }; //formats data to save to text file
            File.WriteAllLines("SaveFile.txt", lines); //saves data to text file
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + scene); //moves on to next scene
        }
        else
        {
            Debug.Log("Match start failed. Error #" + www.downloadHandler.text); //outputs error message

        }
        www.Dispose(); //closes connection
    }
}
