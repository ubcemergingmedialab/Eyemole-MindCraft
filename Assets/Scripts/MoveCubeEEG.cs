using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeEEG : MonoBehaviour {

	public GameObject cube;
	public float maxHeight = 2f;

	// Update is called once per frame
	void Update() {

		if (EEGData.GetEEGData()[1] != 0) {
			Vector3 currentPosition = cube.transform.position;
			Vector3 newPosition = new Vector3(currentPosition.x, EEGData.GetRelativeFrequency(EEGData.EEG_BANDS.ALPHA) * maxHeight, currentPosition.z);
			cube.transform.position = Vector3.Lerp(currentPosition, newPosition, Time.deltaTime);
		}
	}
}