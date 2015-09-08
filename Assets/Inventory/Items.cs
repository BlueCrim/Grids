using UnityEngine;
using System.Collections.Generic;

public class Items : MonoBehaviour 
{
	public List<Sword> swordInspector;
	private static List<Sword> swords;
	private static int idCount;

	public List<PlatformItem> platforms;

	public static Items instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		swords = swordInspector;
		idCount = 0;
	}

	public static int getID()
	{
		Items.idCount++;
		return idCount;
	}

	public static Sword getSword(int id)
	{
		Sword sword = new Sword ();
		sword.image = Items.swords [id].image;
		sword.width = Items.swords [id].width;
		sword.height = Items.swords [id].height;
		return sword;
	}

	public static PlatformItem getPlatform(int id)
	{
		PlatformItem platform = new PlatformItem ();
		platform.image = Items.instance.platforms [id].image;
		platform.width = Items.instance.platforms [id].width;
		platform.height = Items.instance.platforms [id].height;
		platform.type = id;
		return platform;
	}
}
