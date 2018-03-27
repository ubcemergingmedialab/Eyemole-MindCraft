using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlaySoundOnGrabbed : MonoBehaviour {

    private AudioSource grabSound;
    private VRTK_InteractableObject interactableObject;

    // Use this for initialization
    void Start () {

        grabSound = GetComponent<AudioSource>();

        // With Vive / Oculus controllers
        interactableObject = GetComponentInParent<VRTK_InteractableObject>();
        interactableObject.InteractableObjectGrabbed += new InteractableObjectEventHandler(PlaySound);
    }

    void PlaySound (object sender, InteractableObjectEventArgs e) {
        grabSound.Play();
    }

}
