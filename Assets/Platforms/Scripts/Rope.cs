using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour {

	public int length;
	public GameObject ropeBits;

	public LayerMask platformLayer;

	private BoxCollider2D thisCollider;

	private bool hitPlatform;

	// Use this for initialization
	void Awake () {
		platformLayer = Inventory.instance.platformLayer;
		thisCollider = GetComponent<BoxCollider2D> ();

		hitPlatform = false;

		StartCoroutine ("Rise");
	
	}

	IEnumerator Rise()
	{
		Vector3 smallIncrement = Vector3.up / 5;



		for (int i = 0; i < (length * 5); i++) {
			if (!hitPlatform) {
				this.transform.position += smallIncrement;
			} else {
				break;
			}
			yield return new WaitForSeconds (0.03f);
		}

		if (!hitPlatform) {
			gameObject.GetComponent<Pickupable>().TurnToPickup(transform.position);
		}
		StartCoroutine ("UnfurlRope");
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.tag == "Platform") {
			hitPlatform = true;
		}
	}

	IEnumerator UnfurlRope()
	{
		Vector3 pos = this.transform.position;

		for (int i =  1; i <= length; i++) {
			GameObject temp = (GameObject)Instantiate (ropeBits, new Vector3 (pos.x, pos.y - i), Quaternion.identity);
			temp.gameObject.transform.SetParent (this.transform);
			yield return new WaitForSeconds(0.1f);
		}

		//set collider size based on length
		thisCollider.size = new Vector2 (1f, length + 1f);
		thisCollider.offset = new Vector2 (0f, length / (-2f));
	}
}
