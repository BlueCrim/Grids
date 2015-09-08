using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour {

	public GameObject pickupVersion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TurnToPickup()
	{
		Instantiate(pickupVersion, transform.position, Quaternion.identity);

		Destroy (this.gameObject);
	}
}
