using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
	private bool fadeIn = true;
	private bool fadeOut = false;
	private float fadeSpeed = 3f;
	private Image fadeImage; //the image for the fader to use for transitions
    string destinationScene;

	// Use this for initialization
	void Start ()
	{
		fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        fadeImage.color = Color.black;
	}

	// Update is called once per frame
	void Update ()
	{
        //object starts with fadeIn set to true
		if(fadeIn){
			FadeIn();
		}
        //fadeOut becomes true after GoToScene is called
		if(fadeOut){
			FadeOut();
		}
	}

    //call this function to begin scene transition
    public void GoToScene(string whereTo)
    {
        fadeImage.enabled = true;
        fadeOut = true;
        destinationScene = whereTo;
    }

    //called every frame during Update() if fadeIn is true
    void FadeOut()
	{
		fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed  * Time.deltaTime);
        //if this lerp brings the fade color close to opaque, finish and load the new scene
        if (fadeImage.color.a >= .95f)
        {
            fadeOut = false;
            SceneManager.LoadScene(destinationScene);
        }
    }

    //called every frame during Update() if fadeIn is true
	void FadeIn()
	{
        fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
        //if this lerp brings the fade color close to clear, finish and disable the image
        if (fadeImage.color.a <= 0.05){
			fadeImage.color = Color.clear;
			fadeImage.enabled = false;
			fadeIn = false;
		}
	}	
}

