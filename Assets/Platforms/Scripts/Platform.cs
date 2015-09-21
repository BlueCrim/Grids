using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour 
{
	public bool isWall;
	public bool isFloor;
	public bool isCeiling;

	public bool isAWall(){
		return isWall;
	}

	public bool isAGround(){
		return isFloor;
	}

	public bool istACeiling(){
		return isCeiling;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
