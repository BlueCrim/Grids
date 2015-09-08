using UnityEngine;
using System.Collections;

public class WorldItem : MonoBehaviour {
	public Sprite image;
	public int itemID;

	private Rigidbody2D rigidBody2D;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().sprite = image;
		rigidBody2D = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player"))
		{
			if(Inventory.instance.AddToInventory(Items.getPlatform(itemID)))
			{
				Destroy(this.gameObject);
			}
		}

		if (other.tag == "Platform") {
			rigidBody2D.gravityScale = 0;
			rigidBody2D.velocity = Vector2.zero;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Platform") {
			rigidBody2D.gravityScale = 1;
		}
	}
}
