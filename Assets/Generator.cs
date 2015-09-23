using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {
	
	public static Generator instance;

	public List<GameObject> chunks;
	public GameObject player;

<<<<<<< HEAD
	private bool[,] occupied = new bool[1024,1024];

	// Use this for initialization
	void Awake () {

		instance = this;

		for (int i = 0; i < 1024; i++) {
			for(int j = 0; j < 1024; j++){
				occupied[i, j] = false;
			}
		}

		int count = 0;
		int retries = 0;

		//make a random chunk
		GameObject first = Instantiate (chunks [Random.Range (0, chunks.Count)]);

		//place player above a random exitpoint
		player.transform.position = first.GetComponent<ChunkInfo> ().exitPoints [Random.Range (0, first.GetComponent<ChunkInfo> ().exitPoints.Count)].transform.position + Vector3.up;

		//place the real platforms
		PlaceRealPlatform[] children = first.GetComponentsInChildren<PlaceRealPlatform> ();
		foreach (PlaceRealPlatform child in children) {
			child.PlaceReal();
		}

=======
	// Use this for initialization
	void Awake () {

		int count = 0;

		//make a random chunk
		GameObject first = Instantiate (chunks [Random.Range (0, chunks.Count)]);
>>>>>>> 555825777638ef40d075f39ad0b3db818291f568
		List<GameObject> ePoints = first.GetComponent<ChunkInfo> ().exitPoints;
		List<Rect> levelBounds = new List<Rect>();
		levelBounds.Add (first.GetComponent<ChunkInfo> ().chunkBounds);

		for (int i = 0; i < ePoints.Count; i++) {
<<<<<<< HEAD
//Debug.Log (ePoints[i].name + " " + ePoints[i].transform.position);
		}

		while (count < 20 && retries < 100) {

			//make a new try
			retries++;

=======
			//Debug.Log (ePoints[i].name + " " + ePoints[i].transform.position);
		}

		while (count < 15) {
>>>>>>> 555825777638ef40d075f39ad0b3db818291f568
			//pick a random exit point
			GameObject tempExit = ePoints[Random.Range(0, ePoints.Count)];

			//pick a random chunk
			GameObject other = chunks [Random.Range (0, chunks.Count)];
			ChunkInfo otherChunkInfo = other.GetComponent<ChunkInfo>();

			//place the bounding box of the chunk beside the tempExit
			Rect otherRect = otherChunkInfo.chunkBounds;

			//decide if place before or after the block
			int xOffset = Random.Range(0f, 1f) > 0.5 ? -1: 1;
			otherRect.x = tempExit.transform.position.x + xOffset;
			otherRect.y = tempExit.transform.position.y;

			//pick a random entrance point in the other block
			GameObject tempEntrance = otherChunkInfo.exitPoints[Random.Range (0, otherChunkInfo.exitPoints.Count)];

			//offset the bounding rect by the entrance position
			otherRect.position -= (Vector2)tempEntrance.transform.localPosition;

			//Debug.Log(other.name + " " + otherRect + " " + Time.realtimeSinceStartup);

			//find the bounds of the chunk we're checking against
			Rect tempRect = tempExit.GetComponentInParent<ChunkInfo>().chunkBounds;

			bool overlapping = false;
			for(int i = 0; i < levelBounds.Count; i++)
			{
				if(otherRect.Overlaps(levelBounds[i]))
				{
					overlapping = true;
				}
				else{
<<<<<<< HEAD
//Debug.Log (otherRect + " " + levelBounds [i]);
=======
					Debug.Log (otherRect + " " + levelBounds [i]);
>>>>>>> 555825777638ef40d075f39ad0b3db818291f568
				}
			}

			if(!overlapping)
			{
				GameObject addedChunk = (GameObject) Instantiate(other, otherRect.position, Quaternion.identity);

				for(int i = 0; i < otherChunkInfo.exitPoints.Count; i++)
				{
<<<<<<< HEAD
					if(addedChunk.GetComponent<ChunkInfo>().exitPoints[i].name != tempEntrance.name)
					{
						ePoints.Add(addedChunk.GetComponent<ChunkInfo>().exitPoints[i]);
					}
=======
					ePoints.Add(addedChunk.GetComponent<ChunkInfo>().exitPoints[i]);
>>>>>>> 555825777638ef40d075f39ad0b3db818291f568
				}

				//remove the points that connected these 2 chunks
				ePoints.Remove(tempExit);
<<<<<<< HEAD
=======
				ePoints.Remove(tempEntrance);
>>>>>>> 555825777638ef40d075f39ad0b3db818291f568

				count++;

				addedChunk.GetComponent<ChunkInfo>().chunkBounds.position = addedChunk.transform.position;
				levelBounds.Add(addedChunk.GetComponent<ChunkInfo>().chunkBounds);
<<<<<<< HEAD

				//place the real platforms
				children = addedChunk.GetComponentsInChildren<PlaceRealPlatform> ();
				foreach (PlaceRealPlatform child in children) {
					child.PlaceReal();
				}

				retries = 0;
//Debug.Log (ePoints.Count);
			}
=======
				//Debug.Log (ePoints.Count);
			}

>>>>>>> 555825777638ef40d075f39ad0b3db818291f568
		}

		Debug.Log ("Number of chunks: " + count);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
