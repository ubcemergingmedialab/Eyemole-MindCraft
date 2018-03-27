using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMusicWithEEG : MonoBehaviour {

    private AudioSource audio;
    private float timeLastUpdated = 0f;
    private const float timeBetweenUpdates = 1f;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        audio.PlayDelayed(5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - timeLastUpdated >= timeBetweenUpdates) {
            float alphaThetaRatio = EEGData.GetAverage(EEGData.EEG_BANDS.ALPHA) / EEGData.GetAverage(EEGData.EEG_BANDS.THETA);
            alphaThetaRatio = Mathf.Clamp(alphaThetaRatio, -2.9f, 2.9f);
            audio.pitch = alphaThetaRatio;
            timeLastUpdated = Time.time;
        }

    }
}
