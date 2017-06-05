using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJson : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Thieu Mao");
		ConnectionManager.Instance.CheckConnection("1https://www.google.com.vn/", CheckInternetCallback);
	}

	void CheckInternetCallback(bool isInternet) {
		if (isInternet) {
			print ("Co internet");
		} else {
			print ("Mat internet");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
