using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	public static PlayerInfo instance;

	// Use this for initialization
	void Awake () {
		PlayerInfo.instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
