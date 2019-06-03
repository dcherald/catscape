using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGHandlerScript : MonoBehaviour
{
    public GameObject[] bgArray; //array of background images that will fade through eachother
    public float transitionSpeed; //recommended something like .001f;

    private int currentBG = 0; //the current index of the bgArray
    private int nextBG; //the bg to transition to from currBG
    private Image chargeBarBG;
    private Image chargeBarFill;

    // Start is called before the first frame update
    void Start()
    {
        nextBG = currentBG + 1;
        GameObject chargeBarBGObj = GameObject.Find("ChargeBarBG");
        if(chargeBarBGObj != null)
        {
            //set the chargeBar sprite to be the same as the current background, to be aesthetically pleasing
            chargeBarBG = chargeBarBGObj.GetComponent<Image>();
            chargeBarBG.sprite = bgArray[currentBG].GetComponent<SpriteRenderer>().sprite;
            chargeBarFill = GameObject.Find("ChargeBarFill").GetComponent<Image>();
            chargeBarFill.sprite = bgArray[currentBG].GetComponent<SpriteRenderer>().sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get the current background
        SpriteRenderer currentBGSprite = bgArray[currentBG].GetComponent<SpriteRenderer>();
        //get the color and reduce its alpha level slightly
        Color currentColor = currentBGSprite.color;
        currentColor.a -= transitionSpeed;
        currentBGSprite.color = currentColor;
        //if the alpha is almost entirely transparent, change backgrounds
        if (currentColor.a < .05f)
        {
            currentBGSprite.enabled = false;
            //move on to the next background
            currentBG++;
            //rollover to first index if we're at the end
            if (currentBG == bgArray.Length)
            {
                currentBG = 0;
            }
            //bring the new background to the front z index to show it
            bgArray[currentBG].transform.localPosition = new Vector3(0, 0, 1f);
            //get the next background and move it to a z index slightly behind
            //the current one, so it will show as the current on becomes transparent
            nextBG = currentBG + 1;
            if (nextBG == bgArray.Length)
            {
                nextBG = 0;
            }
            bgArray[nextBG].transform.localPosition = new Vector3(0, 0, 1.1f);
            bgArray[nextBG].GetComponent<SpriteRenderer>().enabled = true;
            //set the next background to have an opaque alpha. (reuse currentColor var for performance)
            currentColor = bgArray[nextBG].GetComponent<SpriteRenderer>().color;
            currentColor.a = 1f;
            bgArray[nextBG].GetComponent<SpriteRenderer>().color = currentColor;
            if (chargeBarBG != null)
            {
                chargeBarBG.sprite = bgArray[currentBG].GetComponent<SpriteRenderer>().sprite;
                chargeBarFill.sprite = bgArray[currentBG].GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
