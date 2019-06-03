using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameHandlerScript : MonoBehaviour {

	public Transform surface;
	public int numberOfObjects;
	public Vector3 startPos = new Vector3(-7f,-5f,-2f);
    public float surfaceWidth = .8f;

	private Vector3 nextPos;
	private bool lastWasGap = true; //did the last piece recycled have a gap?
	private Queue<Transform> objectQueue;
	private float minGap = 1, maxGap = 3; //the minimum and maximum size for the gap between platforms
	private GameObject cat;
    private RunnerScript runnerScript;
    private GameObject retryButton;

    // Use this for initialization
    void Awake () 
	{
		objectQueue = new Queue<Transform>(numberOfObjects);
		nextPos = startPos;
        //spawn the objects that will make up the "infinite" running surface
		for (int i = 0; i < numberOfObjects; i++){
            Vector3 spawnPos = new Vector3(startPos.x + (surfaceWidth * i), startPos.y, startPos.z);
			Transform newSurface = (Transform) Instantiate(surface, spawnPos, Quaternion.identity);
			objectQueue.Enqueue(newSurface);
            nextPos = spawnPos;
		}
		cat = GameObject.Find("Cat");
        runnerScript = cat.GetComponent<RunnerScript>();
        retryButton = GameObject.Find("RetryButton");
        retryButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () 
	{
		//if the next surface chunk in the queue is off the screen, recycle it
		if(objectQueue.Peek().position.x < cat.transform.position.x - 9f){
			Recycle();
		}
	}

	void Recycle()
	{
		//determine randomly whether to place a gap
		bool isGap = false;
		float gapSize = 0;
		if(lastWasGap){
			lastWasGap = false;
		}
		else{
			isGap = UnityEngine.Random.Range(0,5) == 0 ? true : false;
			//randomly determine gap size
			if(isGap){
				float speedModifier = (float) Math.Floor(runnerScript.GetSpeedUpCount());
				gapSize = UnityEngine.Random.Range(minGap * speedModifier, maxGap * speedModifier);
                lastWasGap = true;
            }
		}
		//determine spawn location for next piece based on gap
		Vector3 pos = nextPos;
		pos.x += (surfaceWidth * gapSize);
		//dequeue surface leaving the screen and move it
		Transform newSurface = objectQueue.Dequeue();
		newSurface.localPosition = pos;
		nextPos.x = pos.x + surfaceWidth;
		//enqueue the surface at the end
		objectQueue.Enqueue(newSurface);
	}

    public void GameOver()
    {
        StartCoroutine(EndGame());
    }

    public IEnumerator EndGame()
    {
        //game over, disable charge bar, move score display to center
        GameObject.Find("ChargeBarBG").SetActive(false);
        Text distanceDisplay = GameObject.Find("DistanceDisplay").GetComponent<Text>();
        while (distanceDisplay.transform.position.y > 1.5f)
        {
            Vector3 pos = distanceDisplay.transform.position;
            pos.y -= .05f;
            distanceDisplay.transform.position = pos;
            Vector3 scale = distanceDisplay.transform.localScale;
            scale.y += .0001f;
            scale.x += .0001f;
            distanceDisplay.transform.localScale = scale;
            yield return new WaitForSeconds(.007f);
        }
        retryButton.SetActive(true);
    }
}
