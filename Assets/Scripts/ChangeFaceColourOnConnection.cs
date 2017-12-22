﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Leap.Unity.Interaction;

public class ChangeFaceColourOnConnection : MonoBehaviour {

    private Material material;
    private const int SAMPLE_RATE = 256;
    private VRTK_InteractableObject interactableObject; //For Vive controls
	private InteractionBehaviour intObj; //For Leap
	private double lastEEGreceived;
    private double currentEEGreceived;
	private Color currentColour;
	private Color newColour;

    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().material;
		material.shaderKeywords = new string[1] { "_EMISSION" };
		interactableObject = GetComponentInParent<VRTK_InteractableObject>();
		intObj = GetComponentInParent<InteractionBehaviour>();
		lastEEGreceived = 0.0;
        currentEEGreceived = 0.0;
		currentColour = material.color;
		newColour = material.color;
    }
	
	// Update is called once per frame
	void Update () {

        if (interactableObject.IsGrabbed() || intObj.isGrasped) {
            RecordData();
        }
    }

    void RecordData() {
        lastEEGreceived = currentEEGreceived;
        currentEEGreceived = EEGData.eegData[1];
		currentColour = material.color;
		if (currentEEGreceived != lastEEGreceived) {
			newColour = new Color (1.0f - currentColour.r, 1.0f - currentColour.g, 1.0f - currentColour.b);
		} else {
			newColour = currentColour;
		}
		material.SetColor("_EmissionColor", Color.Lerp(currentColour, newColour, Time.deltaTime / Time.fixedDeltaTime));
    }
}
