using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	private Rigidbody2D rigidBody2D;

	[HideInInspector]
	public bool settled; //has this ladder come to a stable stop
	
	// Use this for initialization
	void Start () {
		rigidBody2D = GetComponent<Rigidbody2D> ();
		settled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isALadder(){
		return true;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Platform") {
			rigidBody2D.gravityScale = 0;
			rigidBody2D.velocity = Vector2.zero;
			transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
			settled = true; //ladder has come to a stop
		}
		if (other.tag == "Ladder") {
			if(!other.gameObject.GetComponent<Ladder> ().settled)
			{
				other.gameObject.GetComponent<Pickupable>().TurnToPickup(other.transform.position);
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Platform") {
			rigidBody2D.gravityScale = 1;
		}
	}
}
