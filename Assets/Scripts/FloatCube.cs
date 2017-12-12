using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatCube : MonoBehaviour {

    public float maxAlpha = 2;
    public float minAlpha = -2;
    public float maxHeight = 2;
    public float minHeight = 0;
    public GameObject cube;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float averageAlpha = (float) (EEGData.alphaData[1] + EEGData.alphaData[2]) / 2;
        float currentAlpha = Mathf.Clamp(averageAlpha, minAlpha, maxAlpha);
        float newY = (currentAlpha - minHeight) / (maxHeight - minHeight);
        Vector3 currentPosition = cube.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, newY, currentPosition.z);
        cube.transform.position = Vector3.Lerp(currentPosition, newPosition, 0.1f);
	}
}
