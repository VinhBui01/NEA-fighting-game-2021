using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class Registration : MonoBehaviour
{
    public TMP_InputField nameField; // input box for Username attatched
    public TMP_InputField passwordField; // input box for Password attatched
    public StartManager StartManager;

    public Button submitButton; //Submit Button and whether it has been clicked attatched
    public int LoginType; //Login Type is a number that differentiates whether we are logging in on registering
    
    public void CallRegister() // this subroutine is attatched to submit button
    {
        if (LoginType == 0) //logintype 0 is registration
        {
            
            StartCoroutine(Register());

        }
        else if (LoginType > 0) StartCoroutine(Login(LoginType));
    }
    public void SetLoginType(int num)
    {
        LoginType = num;
    }

    IEnumerator Register()//creates new record in user table
    {
        WWWForm form = new WWWForm(); //creating an empty form
            form.AddField("name", nameField.text); //attatching username to form
            form.AddField("password", passwordField.text); //attatching password to form
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form); //start communnication with web server
        yield return www.SendWebRequest(); //wait for response
        if (www.downloadHandler.text == "000") //000 is response code which means registration was successful
        {
            Debug.Log("Registration Successful");

        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.downloadHandler.text); //output error code
        }
        www.Dispose(); //close connection
        
    }

    IEnumerator Login(int logintype)//checks if username and password are valid and returns userID
    {
        WWWForm form = new WWWForm(); //creating an empty form
        form.AddField("name", nameField.text); //attatching username to form
        form.AddField("password", passwordField.text); //attatching password to form
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form); //start communnication with web server
        yield return www.SendWebRequest(); //wait for response
        if ((www.downloadHandler.text).Split('\t')[0] == "100") //100 is response code which means registration was successful
        {
            Debug.Log("Successful Login");
            //code below attatches PlayerID to start manager variables
            if (LoginType == 1)
            {
                StartManager.P1ID = (www.downloadHandler.text).Split('\t')[1];
            }
            else if (LoginType == 2)
            {
                StartManager.P2ID = (www.downloadHandler.text).Split('\t')[1];
            }
        }
        else
        {
            Debug.Log("User login. Error #" + www.downloadHandler.text); //output error code
        } 

        www.Dispose(); //close connection
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8); //if password/username is incorrect length submit button is unavailable
    }
}
