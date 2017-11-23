using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ChangeFaceColourOnConnection : MonoBehaviour {

    private Material material;
    private const int SAMPLE_RATE = 256;
    private VRTK_InteractableObject interactableObject;
    private double lastEEGreceived;
    private double currentEEGreceived;

    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().material;
        interactableObject = GetComponentInParent<VRTK_InteractableObject>();
        lastEEGreceived = 0;
        currentEEGreceived = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (interactableObject.IsGrabbed()) {
            RecordData();
        }
    }

    void RecordData() {
        lastEEGreceived = currentEEGreceived;
        currentEEGreceived = EEGData.eegData[1];
        if (currentEEGreceived != lastEEGreceived) {
            Color currentColour = material.color;
            Color newColour = new Color(1.0f - currentColour.r, 1.0f - currentColour.g, 1.0f - currentColour.b);
            material.color = Color.Lerp(currentColour, newColour, 0.1f);
        }
    }
}
