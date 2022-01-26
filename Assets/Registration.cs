using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class Registration : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    public StartManager StartManager;

    public Button submitButton;
    public int LoginType;
    private string LoginSuccess;
    public void CallRegister()
    {
        Debug.Log("click");
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

    IEnumerator Login(int logintype)//checks if username and password are valid and returns userID
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/login.php", form);
        yield return www.SendWebRequest();
        if ((www.downloadHandler.text).Split('\t')[0] == "100")
        {
            LoginSuccess = (www.downloadHandler.text).Split('\t')[1];
            Debug.Log("Successful Login");
            if (LoginType == 1)
            {
                StartManager.P1ID = LoginSuccess;
            }
            else if (LoginType == 2)
            {
                StartManager.P2ID = LoginSuccess;
            }
        }
        else
        {
            Debug.Log("User login. Error #" + www.downloadHandler.text);
        }

        www.Dispose();
    }

    private void Update()
    {
        
    }
    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}