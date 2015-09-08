using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public Vector3 pos, preMove;                        // For movement
	private float speed = 5.0f;                         // Speed of movement
	private float jumpTime;

	public bool falling, moving;
	public bool onGround, onLadder;

	public LayerMask platformLayer;
	public LayerMask playerLayer;

	private int frameDelay, frameDelayValue = 2;

	void Start () {
		pos = transform.position;          // Take the initial position
		falling = false;
		moving = false;
		onGround = true;
		onLadder = false;
		frameDelay = frameDelayValue;
	}

	void Update()
	{

	}

	void FixedUpdate () {
		if (Vector3.SqrMagnitude(transform.position - pos) < 0.0001) {
			moving = false;
		}

		if (!onLadder && !onGround) {
			falling = true;
		} else {
			falling = false;
		}
		
		if (falling && !moving) 
		{
			if(frameDelay == 0)
			{
				pos += Vector3.down;
				moving = true;
				frameDelay = frameDelayValue;
			}
			frameDelay--;
		}
		if(Input.GetKey(KeyCode.A) && 	//input
		   !moving) 					//condition
		{        						// Left
			pos = transform.position;   
			pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y));
			preMove = pos;
			pos += Vector3.left;
			moving = true;
		}
		if(Input.GetKey(KeyCode.D) && 	//input
		   !moving) 					//condition
		{						        // Right
			pos = transform.position;
			pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y));
			preMove = pos;
			pos += Vector3.right;
			moving = true;
		}

		if(Input.GetKey(KeyCode.W) && 	//input
		   !moving && onLadder) 		//condition
		{						        // Up
			pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y));
			pos += Vector3.up;
			moving = true;
		}

		if(Input.GetKey(KeyCode.S) && 				//input
		   !moving && onLadder && !onGround) 		//condition
		{						        			// Down
			pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y));
			pos += Vector3.down;
			moving = true;
		}

		transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);   // Move there


	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Ladder" && Vector3.SqrMagnitude(transform.position - other.transform.position) < 0.1) {
			onLadder = true;
			frameDelay = frameDelayValue;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Ladder") {
			onLadder = false;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log ("Hit the ground");
		pos = transform.position;
		falling = false;
	}

	void OnCollisionStay2D(Collision2D other){
		Platform platform = other.collider.GetComponent<Platform> ();
		if (platform != null) {
			if(platform.isAGround())
			{
				onGround = true;
				falling = false;
			}
			if(platform.isAWall())
			{
				Debug.Log("Jitteryness");
				pos = preMove;
			}
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		onGround = false;
	}
}