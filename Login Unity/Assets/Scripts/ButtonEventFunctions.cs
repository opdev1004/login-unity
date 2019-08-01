﻿using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;

public class ButtonEventFunctions : MonoBehaviour
{
	public GameObject loginPanel;
	public GameObject registerPanel;
	private string server = "http://127.0.0.1:5000/";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void CancelRegistration(){
		loginPanel.SetActive(true);
		registerPanel.SetActive(false);
	}

	public void OpenRegistrationPanel(){
		loginPanel.SetActive(false);
		registerPanel.SetActive(true);
	}

	public void Register(){
		TMP_InputField inpuptID = GameObject.Find("User Name").GetComponent(typeof(TMP_InputField)) as TMP_InputField;
		TMP_InputField inputPswd = GameObject.Find("Password").GetComponent(typeof(TMP_InputField)) as TMP_InputField;
		TMP_InputField inputCpswd = GameObject.Find("Confirm Password").GetComponent(typeof(TMP_InputField)) as TMP_InputField;
		string id = inpuptID.text;
		string pswd = inputPswd.text;
		string cpswd = inputCpswd.text;

		if(checkConfirm(pswd, cpswd))
		{
			User user = new User(id, pswd);
			string userData = JsonUtility.ToJson(user);
			StartCoroutine(RegisterResponse(userData));

			inpuptID.text = "";
			inputPswd.text = "";
			inputCpswd.text = "";
			loginPanel.SetActive(true);
			registerPanel.SetActive(false);
		}
		else
		{
			print("The password and confirm password must be same,");
		}
	}

	private IEnumerator RegisterResponse(string userData)
	{
		UnityWebRequest uwr = new UnityWebRequest(server + "register", "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(userData);
		uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
	    uwr.SetRequestHeader("Content-Type", "application/json");
		yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("POST done!");
			StringBuilder sb = new StringBuilder();
            foreach (System.Collections.Generic.KeyValuePair<string, string> dict in uwr.GetResponseHeaders())
            {
                sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
            }
            // Print Headers
            print(sb.ToString());

            // Print Body
            print(uwr.downloadHandler.text);
        }
	}

	bool checkConfirm(string x, string y){
		if(x == y)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
