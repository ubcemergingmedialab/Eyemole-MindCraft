using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class GateTeleport : MonoBehaviour {

	private VRTK_InteractableObject interactableObject;

	// Use this for initialization
	void Start () {
		interactableObject = GetComponent<VRTK_InteractableObject> ();
		interactableObject.InteractableObjectTouched += new InteractableObjectEventHandler (BackToMainMenu);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void BackToMainMenu(object sender, InteractableObjectEventArgs e){
		SceneManager.LoadScene (0);
	}
}
