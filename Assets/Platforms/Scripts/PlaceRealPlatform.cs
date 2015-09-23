using UnityEngine;
using System.Collections;

public class PlaceRealPlatform : MonoBehaviour {

	public GameObject realPlatform;

	// Use this for initialization
	public void PlaceReal () {
		Instantiate (realPlatform, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
	}

}
