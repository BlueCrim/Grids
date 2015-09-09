/* Features:
 * Customizably sized inventory
 * Made up of a series of slots laid over an image
 * Each item can be a number of slots
 * Draw an image on mouse while moving
 * Click move as opposed to drag move
 * Swapping items
 * 
 * */

/* TODO:
 * Stackable items
 * 
 * */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour 
{
	public static Inventory instance;

	public Texture2D image, highlight;
	public Rect position;
	
	public int slotX, slotY;	//offset of the first slot (top-left) from the edge of image

	public GameObject character;	//reference to the player character

	public List<Item> items = new List<Item>();
	public Slot[,] slots;
	
	public LayerMask platformLayer;
	public LayerMask playerLayer;
	
	public Toggle pickupToggle;

	public List<GameObject> platforms;

	[HideInInspector]
	public bool pickupMode;		//pickup or place item mode

	private int slotSizeW = 39;	//how many across each slot is
	private int slotSizeH = 34;	//how high each slot is
	
	private int numSlotsW = 14;	//how many slots across in the inventory
	private int numSlotsH = 7;	//how many slots high in inventory

	private Item temp;			//item being operated on
	private Vector2 selected;

	private bool movingItem;	//are we moving an item?

	private RaycastHit2D hitPlatform, hitPlayer; //raycast has hit platform or player

	// Use this for initialization
	void Awake () 
	{
		slots = new Slot[numSlotsW,numSlotsH];
		setSlots ();
		movingItem = false;
		pickupToggle.onValueChanged.AddListener (TogglePickupMode);
		Inventory.instance = this;
	}

	public void TogglePickupMode (bool pickup)
	{
		pickupMode = pickup;
	}

	void setSlots()
	{
		for(int x = 0; x < numSlotsW; x++)
		{
			for(int y = 0; y < numSlotsH; y++)
			{
				slots[x, y] = new Slot(new Rect(slotX + slotSizeW * x, slotY + slotSizeH * y, slotSizeW, slotSizeH));
			}
		}
	}

	void OnGUI()
	{
		drawInventory();
		//drawSlots();
		drawItem ();
		detectGUIAction ();
		drawTempItem ();
	}

	void drawSlots()
	{
		for(int x = 0; x < 14; x++)
		{
			for(int y = 0; y < 7; y++)
			{
				slots[x, y].draw(position.x, position.y);
			}
		}
	}

	//draw item image
	void drawItem()
	{
		for (int i = 0; i < items.Count; i++) 
		{
			GUI.DrawTexture(new Rect(slotX + position.x + items[i].x * slotSizeW,
			                         slotY+ position.y + items[i].y * slotSizeH,
			                         items[i].width * slotSizeW,
			                         items[i].height * slotSizeH), 
			                items[i].image);
		}
	}

	public bool AddToInventory (Item item)
	{
		for(int x = 0; x < 14; x++)
		{
			for(int y = 0; y < 7; y++)
			{
				if(addItem(y, x, item))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool addItem(int x, int y, Item item)
	{
		//check that we don't leave the bounds of inventory
		if (x + item.width > numSlotsW) {
			return false;
		}
		if (y + item.height > numSlotsH) {
			return false;
		}

		//check if slots item will take up is occupied
		for (int i = x; i < item.width + x; i++) 
		{
			for(int j = y; j < item.height + y; j++)
			{
				if(slots[i, j].occupied)
				{
					Debug.Log("No Bueno, no add-o");
			        return false;
				}
			}
		}

		//if not,
		//each slot occupied should have a reference to the item that is occupying it
		item.x = x;
		item.y = y;

		//each item should also have a unique id
		item.id = Items.getID ();

		// add item, mark slots as occupied
		items.Add(item);
		for (int i = x; i < item.width + x; i++) 
		{
			for(int j = y; j < item.height + y; j++)
			{
				slots[i, j].occupied = true;
				slots[i, j].item = item;
			}
		}
		//Debug.Log ("Adding successful");
		return true;
	}

	void removeItem(Item item)
	{
		//mark the space that the item was in as unoccupied
		for (int i = item.x; i < item.width + item.x; i++) 
		{
			for(int j = item.y; j < item.height + item.y; j++)
			{
				slots[i, j].occupied = false;
				slots[i, j].item = null;
			}
		}

		items.Remove (item);
	}

	//disable other controls
	void detectGUIAction()
	{
		//try using rect contains if necessary
		if (Input.mousePosition.x > position.x && Input.mousePosition.x < position.x + position.width) {
			if (Screen.height - Input.mousePosition.y > position.y && Screen.height - Input.mousePosition.y < position.y + position.height) {
				detectMouseAction ();
				//mark controls as busy
			}

			//not within the inventory menu. try doing something in the world
			else {

				//make the raycast to determine if we've hit an area we can place in or a platform
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
				hitPlatform = Physics2D.GetRayIntersection(ray, Mathf.Infinity, platformLayer);
				hitPlayer = Physics2D.GetRayIntersection(ray, Mathf.Infinity, playerLayer);

				if (pickupMode) 
				{
					PickUpItemFromWorld();
				}
				else {
					//check if we're holding an item
					if (temp != null) {
						//see if we should place it in the world
						PlaceItemInWorld ();
					}
				}
			}
		}


		//mark controls as not busy
	}

	void PickUpItemFromWorld()
	{
		if (Event.current.isMouse && Input.GetMouseButtonDown(0)){
			
			//check if we're near the character
			if(hitPlayer.collider != null && hitPlayer.collider.tag.Equals("PlacementAid"))
			{
				//as long as there is a platform
				if(hitPlatform.collider != null)
				{
					//find the pickupable script and activate it
					Pickupable target = hitPlatform.collider.GetComponent<Pickupable>();
					if(target != null)
					{
						target.TurnToPickup(hitPlayer.collider.transform.position);
					}
				}
			}
		}
	}

	//try to place an item from inventory into world
	void PlaceItemInWorld()
	{
		if (Event.current.isMouse && Input.GetMouseButtonDown(0)){

			//check if we're placing on an existing platform
			if(hitPlatform.collider == null)
			{
				//check if we're near the character
				if(hitPlayer.collider != null && hitPlayer.collider.tag.Equals("PlacementAid"))
				{
					//find the right place the platform
					Vector3 placementPos = hitPlayer.collider.transform.position;
					Instantiate(platforms[temp.type], placementPos, Quaternion.identity);

					//we're done, remove it from inventory
					temp = null;
					movingItem = false;
				}
			}
		}
	}

	void detectMouseAction()
	{

		//check to see if mouse is in any slot in inventory
		for(int x = 0; x < numSlotsW; x++)
		{
			for(int y = 0; y < numSlotsH; y++)
			{
				Rect slot = new Rect(position.x + slots[x, y].position.x,
				                     position.y + slots[x, y].position.y,
				                     slotSizeW,
				                     slotSizeH);
				if(slot.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
				{
					GUI.DrawTexture(slot, highlight);
					if (Event.current.isMouse && Input.GetMouseButtonDown(0))
					{
						//not currently moving item, so check if there is something to move
						if(!movingItem)
						{
							//if the slot has no item, do nothing and return
							if(slots[x, y].item == null) return;

							//keep track of the item's original position
							selected.x = slots[x, y].item.x;
							selected.y = slots[x, y].item.y;
							
							//if selected slot has an item, place it in temp
							temp = slots[x, y].item;

							removeItem(temp);
							//change state to moving an item
							movingItem = true;
						}
						//we are currently in the process of moving an item, so try to place it
						else
						{
							//try to place item in new slot. If fail, place back in original location
							if(!swap (x, y))
							{
								Debug.Log ("Returning item");
								addItem ((int)selected.x, (int)selected.y, temp);
								temp = null;
							}

							//if nothing is in temp, we're no longer moving an item
							if(temp == null)
							{
								movingItem = false;
							}
						}
					}
					//Debug.Log (selected + "               " + secondSelected);
					//Debug.Log (x + " , " + y);
				}
			}
		}
	}

	//check if a sequence of slots is occupied by a single item
	bool checkForFit(int x, int y, int width, int height)
	{
		Item thisItem = null;

		//check that we don't leave the bounds of inventory
		if (x + width > numSlotsW) {
			return false;
		}
		if (y + height > numSlotsH) {
			return false;
		}

		//get the id of the first item we encounter
		for(int i = x; i < x + width; i++)
		{
			for(int j = y; j < y + height; j++)
			{
				if(slots[i, j].item != null)
				{
					thisItem = slots[i, j].item;
				}
			}
		}

		//if there are no items here, it'll fit
		if (thisItem == null)
			return true;

		//loop through the spaces the item would occupy
		for(int i = x; i < x + width; i++)
		{
			for(int j = y; j < y + height; j++)
			{
				//if the slots are occupied and the IDs do not match, then return false
				if(slots[i, j].occupied && thisItem.id != slots[i, j].item.id)
				{
					return false;
				}
			}
		}
		return true;
	}

	//try to swap two items
	bool swap (int x, int y)
	{
		Item swapItem = slots[x, y].item;
		//if the temp item fits in the space suggested, 
		if (checkForFit (x, y, temp.width, temp.height)) {
			//look for an item in the space suggested
			for (int i = x; i < x + temp.width; i++) {
				for (int j = y; j < y + temp.height; j++) {
					//if we've already found an item, break
					if (swapItem != null)
						break;
					else
						swapItem = slots [i, j].item;
				}
			}
		}

		//otherwise, swap failed and return false
		else 
		{
			return false;
		}

		//if there's nothing in the space, just add the temp item
		if (swapItem == null) 
		{
			addItem (x, y, temp);
			temp = null;
		} 
		//otherwise, swap second item into temp and place original into new position
		else 
		{
			removeItem(swapItem);
			addItem(x, y, temp);
			temp = swapItem;
		}
		return true;
	}

	void drawTempItem()
	{
		if (temp != null) 
		{
			GUI.DrawTexture(new Rect(Input.mousePosition.x - 10,
			                         Screen.height - Input.mousePosition.y - 10,
			                         temp.width * slotSizeW,
			                         temp.height * slotSizeH),
			                temp.image);
		}
	}

	void drawInventory()
	{
		position.x = Screen.width*0.5f- position.width*0.5f;	// - Screen.width * 0.5f;
		position.y = 0f;//Screen.height*0.5f - position.height*0.5f;	// - Screen.height * 0.5f;
		GUI.DrawTexture (position, image);
	}
}
