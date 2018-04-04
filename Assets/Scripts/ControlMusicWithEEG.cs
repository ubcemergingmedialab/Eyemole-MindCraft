using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMusicWithEEG : MonoBehaviour {

    private AudioSource audio;
    private float timeLastUpdated = 0f;
    private const float timeBetweenUpdates = 0.1f;
    private float stepSize = 0.001f;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        audio.PlayDelayed(5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - timeLastUpdated >= timeBetweenUpdates) {
            float alphaThetaRatio = Mathf.Pow(10f, EEGData.GetAverage(EEGData.EEG_BANDS.ALPHA))/ Mathf.Pow(10f, EEGData.GetAverage(EEGData.EEG_BANDS.THETA));
            if (alphaThetaRatio > 1) {
                audio.pitch += stepSize;
            }
            else {
                audio.pitch -= stepSize;
            }
           
            timeLastUpdated = Time.time;
        }

    }
}
