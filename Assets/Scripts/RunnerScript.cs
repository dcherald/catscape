using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RunnerScript : MonoBehaviour {

	public static float distanceTraveled;
	public static Vector3 dragStartPos;

	public float dashSpeed = 5f;
	private float speedUpCount = 1f;
	private bool dragging = false; //is the mouse clicked and being dragged?
	private bool gameOver = false; //is the game over?
	private GameObject click; //the cursor to show where the mouse was clicked
	private GameObject dragCursor; //the cursor to show where the mouse is during drag
    private float maxDragDistance; //the max units to drag based on total screen space
	private Text distanceDisplay; //the text that displays the current distance traveled
	private GameObject retryButton;
    private Image chargeBar;

	void Start()
	{
		click = GameObject.Find("Click");
		dragCursor = GameObject.Find("DragCursor");
		distanceDisplay = GameObject.Find("DistanceDisplay").GetComponent<Text>();
        chargeBar = GameObject.Find("ChargeBarFill").GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () 
	{
        if (!gameOver)
        {
            //get max dragDistance each frame in case screen size changes for some reason (such as in browser)
            maxDragDistance = Screen.width / 2;
            //display distance traveled
            distanceDisplay.text = "" + distanceTraveled.ToString("n2") + " m";
            //increase speed at regular intervals
            if (distanceTraveled > (100f * speedUpCount))
            {
                speedUpCount++;
                dashSpeed += .5f;
            }
            //always move forward
            //unless there is a wall in front. If a wall exists, the cat has fallen
            //and should stop moving.
            if (!WallPresent())
            {
                transform.Translate(dashSpeed * Time.deltaTime, 0f, 0f);
                //keep track of distance traveled
                distanceTraveled = transform.localPosition.x;
                //if grounded, click and drag to jump
                if (OnGround())
                {
                    //MouseButtonDown works for desktop and mobile
                    if (Input.GetMouseButtonDown(0))
                    {
                        dragging = true;
                        dragStartPos = Input.mousePosition;
                        Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(dragStartPos);
                        clickWorldPos.z = -5; //have click be in front of everything else.
                        click.transform.position = clickWorldPos;
                        click.GetComponent<SpriteRenderer>().enabled = true;
                        dragCursor.transform.position = clickWorldPos;
                        dragCursor.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                //when drag is released, jump
                if (dragging)
                {
                    Vector3 currMousePos = Input.mousePosition;
                    if (dragStartPos.x - currMousePos.x > maxDragDistance)
                    {
                        currMousePos.x = dragStartPos.x - maxDragDistance;
                    }
                    Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(currMousePos);
                    clickWorldPos.z = -5f;
                    dragCursor.transform.position = clickWorldPos;
                    //fill charge bar based on drag amount
                    chargeBar.fillAmount = ((dragStartPos.x - currMousePos.x) / maxDragDistance);
                    if (Input.GetMouseButtonUp(0))
                    {
                        dragging = false;
                        float dragForce = Camera.main.ScreenToWorldPoint(dragStartPos).x - clickWorldPos.x;
                        //dragForce = dragForce > maxDragDistance ? maxDragDistance : dragForce;
                        Vector2 jumpForce = new Vector2(0, 50f * dragForce);
                        print(jumpForce);
                        gameObject.GetComponent<Rigidbody2D>().AddForce(jumpForce);
                        dragCursor.GetComponent<SpriteRenderer>().enabled = false;
                        click.GetComponent<SpriteRenderer>().enabled = false;
                        chargeBar.fillAmount = 0f;
                    }
                }
            }
            if (transform.position.y < -6)
            {
                GameObject.Find("GameplayHandler").SendMessage("GameOver");
                //can't destroy runner, because camera and canvas are attached to it.
                gameOver = true;
            }
        }
	}

	bool OnGround()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 1f);
		foreach(RaycastHit2D hit in hits){
			if(hit.transform.tag == "surface"){
				return true;
			}
		}
		return false;
	} 

	bool WallPresent()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, .7f);
		foreach(RaycastHit2D hit in hits){
			if(hit.transform.tag == "surface"){
				return true;
			}
		}
		return false;
	}

    // return the amount that the speed has changed
	public float GetSpeedUpCount()
	{
        return speedUpCount;
	}
}
