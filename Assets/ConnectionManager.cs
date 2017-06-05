using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MiniJSON;
using UnityEngine;

public delegate void CallBackFunction(string message);
public delegate void CallBackFunc(bool callback);
public delegate void GameEvent();
public enum Connect
{
	None, Connected
}

public sealed class ConnectionManager : Singleton<ConnectionManager>
{
	void Start()
	{
		DontDestroyOnLoad(this);
	}

	public int LevelBoss = 10001; // Level cua map cua chuc nang boss
	//public static event GameEvent LoadDataCompleted; // Su kien khi Data JSON phan tich thanh cac object thanh cong
	internal int Week; // Tuan hien tai 
	internal int IsCollect; // Phan thuong tuan truoc nguoi dung da nhan chua?

	public static bool IsConnected
	{
		get;
		set;
	}
	public float MaxTimePending = 10f; //Thoi gian cho toi da khi server chua phan hoi.


	public void CheckConnection(string url, CallBackFunc callback)
	{

		StartCoroutine(Connect(url, callback));
	}
	public void OnCheckConnection(bool callback)
	{
		if (callback) { OnConnectSuccessful("Connect Successfully"); }
		else
		{
			OnConnectFailed("Connect Failded");
			print(ConnectionManager.IsConnected);

		}
	}
	public void OnConnectSuccessful(string message)
	{
		////Debug.Log(message);
	}
	public void OnConnectFailed(string message)
	{
		Debug.Log(message);
	}
	public void SynsDataToServer(string url, CallBackFunction callBack)
	{
		StartCoroutine(Connect(url, callBack));
	}
	public void SynsDataToServer(string url, WWWForm form, CallBackFunction callBack)
	{
		//  Debug.Log("SynsDataToServer");
		StartCoroutine(Connect(url, form, callBack));
	}
	public void SynsDataFromServer(string url, CallBackFunction callBack)
	{
		//  Debug.Log("SynsDataFromServer");
		StartCoroutine(Connect(url, callBack));
	}
	public void SynsDataFromServer(string url, WWWForm form, CallBackFunction callBack)
	{
		Debug.Log("SynsDataFromServer");
		StartCoroutine(Connect(url, form, callBack));
	}
	public void OnSynsDataFromServer(string message)
	{
		Debug.Log(message);
		//ParseData(message);
	}
	public void OnSynsDataToServer(string message)
	{
		Debug.Log(message);
		Debug.Log("OnSynsDataToServer");
	}
	public IEnumerator Connect(string url, CallBackFunc callBack)
	{
		WWW www = new WWW(url);
		float timer = 0;
		bool failed = false;
		while (!www.isDone)
		{
			if (timer > MaxTimePending)
			{
				failed = true;
				break;
			}

			timer += Time.deltaTime;
			yield return null;
		}
		if (failed)
		{
			IsConnected = false;
			callBack(false);
		}
		else
		{
			if (string.IsNullOrEmpty(www.error))
			{
				IsConnected = true;
				callBack(true);

			}
			else
			{
				IsConnected = false;
				callBack(false);
				// fireEventConnectFail();
			}
		}
	}
	public IEnumerator Connect(string url, CallBackFunction callBack)
	{
		WWW www = new WWW(url);
		float timer = 0f;
		bool failed = false;
		while (!www.isDone)
		{
			if (timer > MaxTimePending)
			{
				failed = true;
				break;
			}
			timer += Time.deltaTime;
			yield return null;
		}
		if (failed)
		{
			www.Dispose();
			IsConnected = false;
			callBack(null);
		}
		else
		{
			if (string.IsNullOrEmpty(www.error))
			{
				IsConnected = true;
				callBack(www.text);

			}
			else
			{
				IsConnected = false;
				callBack(null);
				// fireEventConnectFail();

			}
		}
	}
	public IEnumerator Connect(string url, WWWForm form, CallBackFunction callBack)
	{
		// kiem tra xem du lieu gui len server dang post hay get 
		var www = form != null ? new WWW(url, form) : new WWW(url);

		var timer = 0f;
		var failed = false;
		// Kiem tra xem gui request len server chua nhan duoc goi dap thi doi den khi het thoi gian pending thi thoat vong lap
		//
		while (!www.isDone)
		{
			if (timer > MaxTimePending)
			{
				failed = true;
				break;
			}

			timer += Time.deltaTime;
			yield return null;
		}
		// failed = true thi huy bo doi tuong ket noi toi server ==> IsConnected = false
		if (failed)
		{
			www.Dispose();
			IsConnected = false;

		}
		else
		{
			//Neu nhu tai duoc tat ca du lieu thanh cong thi goi lai ham callback va set IsConnected = true;
			if (string.IsNullOrEmpty(www.error))
			{

				IsConnected = true;
				callBack(www.text);

			}
			else
			{
				IsConnected = false;
			}
		}
	}




	#region POST

	/// <summary>
	/// gui thong ke sau khi win/lose
	/// </summary>
	/// <param name="_detail">data</param>
	public void SentResult(string _content, string _url)
	{
		WWWForm from = new WWWForm();
		from.AddField("data", _content);
		SynsDataToServer(_url, from, Messtest);
	}

	void Messtest(string mess)
	{
		Debug.Log(mess);
	}



	#endregion



	#region Get

	#endregion

}
