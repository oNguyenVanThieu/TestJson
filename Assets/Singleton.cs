using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = (T)FindObjectOfType(typeof(T));
				if (_instance == null)
				{
					var singleton = new GameObject();
					_instance = singleton.AddComponent<T>();
					singleton.name =  typeof(T).ToString();
					;
				}
			}
			return _instance;
		}
	}
}
