using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using Leap.Unity.Interaction;

public class GateTeleport : MonoBehaviour {

	private VRTK_InteractableObject interactableObject; // If using Vive controllers
	private InteractionBehaviour intObject; // If using Leap

	// Use this for initialization
	void Start () {
		interactableObject = GetComponent<VRTK_InteractableObject> ();
		interactableObject.InteractableObjectTouched += new InteractableObjectEventHandler (BackToMainMenu);

		intObject = GetComponentInParent<InteractionBehaviour>();
		intObject.OnContactBegin += BackToMainMenu;
	}

	void BackToMainMenu() {
		SceneManager.LoadScene(0);
	}

	void BackToMainMenu(object sender, InteractableObjectEventArgs e){
		SceneManager.LoadScene (0);
	}
}
