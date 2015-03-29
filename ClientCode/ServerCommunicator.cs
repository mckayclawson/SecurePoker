using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class ServerCommunicator : MonoBehaviour {

    string postUrl = "http://localhost:" + Server.PortNumber;

	bool waiting = false;

	public void sendSignInRequest(string username, string password) {
		JObject signInObject = new JObject();
		signInObject.Add ("message_type", "sign_in");
		signInObject.Add ("username", username);
		signInObject.Add ("password", password);

		WWWForm form = new WWWForm();
		form.AddField("json", signInObject.ToString());

		Debug.Log(signInObject.ToString());

		WWW www = new WWW(postUrl, form);

		waiting = true;

		StartCoroutine(WaitForRequest(www));
	}

	public void sendRegisterRequest(string username, string password) {
		JObject regObject = new JObject();
		regObject.Add ("message_type", "register");
		regObject.Add ("username", username);
		regObject.Add ("password", password);
		Debug.Log (regObject.ToString());

		WWWForm form = new WWWForm();
		form.AddField("json", regObject.ToString());
		
		WWW www = new WWW(postUrl, form);
		
		waiting = true;
		
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www) {
		yield return www;
		Debug.Log("sending");
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
			waiting = false;
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
}
