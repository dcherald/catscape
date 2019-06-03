using UnityEngine;
using System.Collections;

public class QuickTransitionScript : MonoBehaviour
{
	public float transitionDelay = 1f;
	public string whereTo = "MainMenu";

	private float timer;
	private GameObject fader;

	void Start()
	{
		timer = Time.time;
		fader = GameObject.Find("ScreenFader");
	}

	// Update is called once per frame
	void Update ()
	{
		if(Time.time - timer > transitionDelay){
			fader.SendMessage("EndScene", whereTo);
		}
	}
}
