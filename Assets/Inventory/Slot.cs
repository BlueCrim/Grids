using UnityEngine;
using System.Collections;

[System.Serializable]

public class Slot
{
	public Item item;		//reference to item in this slot
	public bool occupied;	//slot is occupied
	public Rect position; 	//position of slot relative to frame/screen


	public Slot(Rect position)
	{
		this.position = position;
	}

	public void draw (float frameX, float frameY)
	{
		if (item != null) 
		{
			GUI.DrawTexture (new Rect (frameX + position.x, frameY + position.y, position.width, position.height), item.image);
		}
	}
}
