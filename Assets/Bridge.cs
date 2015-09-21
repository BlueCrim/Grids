using UnityEngine;
using System.Collections;

public class Bridge : MonoBehaviour {



	// Use this for initialization
	void Awake () {
		Vector3 playerPos = PlayerInfo.instance.transform.position;
		if (Mathf.Round(transform.position.y) != Mathf.Round(playerPos.y)) {
			GetComponent<Pickupable> ().TurnToPickup(playerPos);
		}

		if (PlayerInfo.instance.transform.position.x > transform.position.x) {
			transform.localScale = new Vector3 (-transform.localScale.x, 1, 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
