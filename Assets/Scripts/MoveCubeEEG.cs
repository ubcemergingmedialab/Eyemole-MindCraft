using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeEEG : MonoBehaviour {

	public GameObject cube;
	public float maxHeight = 2f;
	
	// Update is called once per frame
	void Update () {

		Vector3 currentPosition = cube.transform.position;
		Vector3 newPosition = new Vector3(currentPosition.x, EEGData.GetRelativeAlpha() * maxHeight, currentPosition.z);
		cube.transform.position = Vector3.Lerp(currentPosition, newPosition, Time.deltaTime);
		
	}
}
