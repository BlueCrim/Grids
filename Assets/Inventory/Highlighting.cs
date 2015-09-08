using UnityEngine;
using System.Collections;

public class Highlighting : MonoBehaviour {

	void OnMouseEnter()
	{
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
	}

	void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
	}
}
