using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	public List<GameObject> chunks;

	// Use this for initialization
	void Awake () {

		int count = 0;

		//make a random chunk
		GameObject first = Instantiate (chunks [Random.Range (0, chunks.Count)]);
		List<GameObject> ePoints = first.GetComponent<ChunkInfo> ().exitPoints;
		List<Rect> levelBounds = new List<Rect>();
		levelBounds.Add (first.GetComponent<ChunkInfo> ().chunkBounds);

		for (int i = 0; i < ePoints.Count; i++) {
			//Debug.Log (ePoints[i].name + " " + ePoints[i].transform.position);
		}

		while (count < 15) {
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
					Debug.Log (otherRect + " " + levelBounds [i]);
				}
			}

			if(!overlapping)
			{
				GameObject addedChunk = (GameObject) Instantiate(other, otherRect.position, Quaternion.identity);

				for(int i = 0; i < otherChunkInfo.exitPoints.Count; i++)
				{
					ePoints.Add(addedChunk.GetComponent<ChunkInfo>().exitPoints[i]);
				}

				//remove the points that connected these 2 chunks
				ePoints.Remove(tempExit);
				ePoints.Remove(tempEntrance);

				count++;

				addedChunk.GetComponent<ChunkInfo>().chunkBounds.position = addedChunk.transform.position;
				levelBounds.Add(addedChunk.GetComponent<ChunkInfo>().chunkBounds);
				//Debug.Log (ePoints.Count);
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
