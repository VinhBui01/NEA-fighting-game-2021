using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class Registration : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField passwordField;

    public Button submitButton;
    public string[] P1Login = new string[2];
    public string[] P2Login = new string[2];
    public int LoginType;
    private bool LoginSuccess;
    public void CallRegister()
    {
        if (LoginType == 0) //logintype 0 is registration
        {
            StartCoroutine(Register());

        }
        else if (LoginType == 1)
        {
            LoginSuccess = false;
            StartCoroutine(Login());
            if (LoginSuccess == true)
            {
                P1Login[0] = nameField.text;
                P1Login[1] = passwordField.text;
            }

        }
        else if (LoginType == 2)
        {
            LoginSuccess = false;
            StartCoroutine(Login());
            if (LoginSuccess == true)
            {
                P2Login[0] = nameField.text;
                P2Login[1] = passwordField.text;
            }

        }
        else { Debug.Log(LoginType); }
    }
    public void SetLoginType(int num)
    {
        LoginType = num;
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
            form.AddField("name", nameField.text);
            form.AddField("password", passwordField.text);
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

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == "100")
        {
            LoginSuccess = true;
            Debug.Log("Successful Login");
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.downloadHandler.text);
        }
        www.Dispose();

    }


    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}