using UnityEngine;
using System.Collections;

public class PlaceRealPlatform : MonoBehaviour {

	public GameObject realPlatform;

	// Use this for initialization
	void Awake () {
		Instantiate (realPlatform, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
	}

}
