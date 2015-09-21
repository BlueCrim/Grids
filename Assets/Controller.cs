using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public Vector3 pos, preMove;                        // For movement
	private float speed = 5.0f;                         // Speed of movement
	private float jumpTime;

	public bool falling, moving, canMove;
	public bool onGround, onLadder;

	public LayerMask platformLayer;
	public LayerMask playerLayer;

	public int frameDelay, frameDelayValue = 1;

	void Awake () {
		//pos = transform.position;          // Take the initial position
		falling = false;
		moving = false;
		canMove = false;
		onGround = true;
		onLadder = false;
		frameDelay = frameDelayValue;
	}

	void Update()
	{

	}

	void FixedUpdate () {
		if ((Vector3.SqrMagnitude(transform.position - pos) < 0.0001) && moving) {
			StopMovement();
		}
		
		if (!moving && canMove && !onLadder && !onGround) {
			falling = true;
		} else {
			falling = false;
		}

		if (canMove) {
		
			if (falling) {
				pos = transform.position;   
				pos = new Vector3 (Mathf.Round (pos.x), Mathf.Round (pos.y));
				pos += Vector3.down;
				moving = true;
				//canMove = true;
			}

			//Left
			if (Input.GetKey (KeyCode.A) && //input
				!moving) { 					//condition
				pos = transform.position;   
				pos = new Vector3 (Mathf.Round (pos.x), Mathf.Round (pos.y));
				preMove = pos;
				pos += Vector3.left;
				moving = true;
				//canMove = true;
			}

			//Right
			if (Input.GetKey (KeyCode.D) && //input
				!moving) { 					//condition
				pos = transform.position;
				pos = new Vector3 (Mathf.Round (pos.x), Mathf.Round (pos.y));
				preMove = pos;
				pos += Vector3.right;
				moving = true;
				//canMove = true;
			}

			// Up
			if (Input.GetKey (KeyCode.W) && //input
				!moving && onLadder) { 		//condition						     
				pos = transform.position;   
				pos = new Vector3 (Mathf.Round (pos.x), Mathf.Round (pos.y));
				pos += Vector3.up;
				moving = true;
				//canMove = true;
			}

			//Down
			if (Input.GetKey (KeyCode.S) && 				//input
				!moving && onLadder && !onGround) { 		//condition
				pos = new Vector3 (Mathf.Round (pos.x), Mathf.Round (pos.y));
				pos += Vector3.down;
				moving = true;
				//canMove = true;
			}
		}

		if(!canMove && frameDelay == 0)
		{
			canMove = true;
		}
		else if(!canMove){
			frameDelay--;
		}

		transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);   // Move there



	}

	void StopMovement ()
	{
		canMove = false;
		moving = false;
		frameDelay = frameDelayValue;
		pos = transform.position;   
		pos = new Vector3 (Mathf.Round (pos.x), Mathf.Round (pos.y));
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Ladder" && (transform.position.x - other.transform.position.x) < 0.1) {
			onLadder = true;
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
		if (falling) {
			StopMovement ();
			falling = false;
		}
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