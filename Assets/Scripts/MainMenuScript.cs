using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
	void Start()
	{
		GameObject[] audios = GameObject.FindGameObjectsWithTag("Audio");
		if(audios.Length > 1){
			for(int i = 1; i < audios.Length; i++){
				Destroy(audios[i]);
			}
		}
		DontDestroyOnLoad(GameObject.Find("Audio"));
	}	
}

