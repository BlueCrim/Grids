using UnityEngine;
using System.Collections.Generic;

public class Items : MonoBehaviour 
{
	private static int idCount;

	public List<PlatformItem> platforms;

	public static Items instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		idCount = 0;
	}

	public static int getID()
	{
		Items.idCount++;
		return idCount;
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
