using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Item
{
	public Texture2D image;		//picture of item
	public int width, height;	//number of slots in inventory this item takes up
	public int id;				//unique id of this item
	public int type;			//the type of item this is

	[HideInInspector]
	public int x, y;			//location in inventory

	public abstract void performAction ();
}
